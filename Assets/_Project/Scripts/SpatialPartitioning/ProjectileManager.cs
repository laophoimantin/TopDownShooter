using System.Collections.Generic;
using UnityEngine;


public class ProjectileManager : Singleton<ProjectileManager>, IUpdater
{
    public List<ProjectileSP> activeProjectiles = new();
    [SerializeField] private LayerMask _wallLayer;

    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }
    void OnDisable()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.OnUnassignUpdater(this);
    }

    public void SpawnProjectile(ProjectileSP proj)
    {
        proj.ProjIndex = activeProjectiles.Count;
        activeProjectiles.Add(proj);
    }

    public void DespawnProjectile(ProjectileSP proj)
    {
        int index = proj.ProjIndex;
        if (index < 0 || index >= activeProjectiles.Count || activeProjectiles[index] != proj) return;

        int lastIndex = activeProjectiles.Count - 1;
        ProjectileSP lastProj = activeProjectiles[lastIndex];

        activeProjectiles[index] = lastProj;
        lastProj.ProjIndex = index;

        activeProjectiles.RemoveAt(lastIndex);
        proj.ProjIndex = -1;
    }

    public void OnUpdate()
    {
        if (activeProjectiles.Count == 0) return;

        float dt = Time.deltaTime;

        for (int i = activeProjectiles.Count - 1; i >= 0; i--)
        {
            ProjectileSP proj = activeProjectiles[i];

            proj.LifetimeTimer -= dt;
            if (proj.LifetimeTimer <= 0)
            {
                proj.Die();
                continue;
            }

            Vector3 myPos = proj.CurrentPos;
            myPos += proj.Direction * (proj.Speed * dt);

            if (Physics2D.OverlapCircle(myPos, proj.HitboxRadius, _wallLayer))
            {
                proj.Explode(myPos);
                continue;
            }

            SpatialGrid.Instance.GetNearbyEntities(myPos, proj.HitboxRadius, ref proj.NearbyEnemies);

            bool hasExploded = false;

            for (int j = 0; j < proj.NearbyEnemies.Count; j++)
            {
                MobController victim = proj.NearbyEnemies[j];

                if (proj.HitTargets.Contains(victim)) continue;

                float sqrDist = ((Vector2)victim.MovementSP.CurrentPos - (Vector2)myPos).sqrMagnitude;

                if (sqrDist <= proj.SqrRadius)
                {
                    proj.HitTargets.Add(victim);

                    Vector2 finalKnockback = (Vector2)proj.Direction * proj.KnockbackForce;

                    victim.TakeDamage(proj.Damage, finalKnockback);

                    if (proj.PierceCount > 0)
                    {
                        proj.PierceCount--;
                    }
                    else
                    {
                        proj.Explode(myPos);
                        hasExploded = true;
                        break;
                    }
                }
            }

            if (!hasExploded)
            {
                proj.transform.position = myPos;
                proj.CurrentPos = myPos;
            }
        }

    }
}