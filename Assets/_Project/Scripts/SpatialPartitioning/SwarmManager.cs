using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class SwarmManager : Singleton<SwarmManager>, IUpdater
{
    public List<MobControllerSP> activeMobs = new();
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerController _playerController;
    
    [Header("Swarm Settings")]
    [SerializeField] private float _separationRadius = 1.5f;
    [SerializeField] private float _separationWeight = 3f;
    
    // private TransformAccessArray _transformAccessArray;
    // private NativeArray<Vector3> _separationForces;
    
    private float _radius;
    private float _sqrRadius;
    private float _sepWeight;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _radius = _separationRadius;
        _sqrRadius = _separationRadius * _separationRadius;
        _sepWeight = _separationWeight;
    }
#endif
    
    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.OnUnassignUpdater(this);
    }

    public void RegisterMob(MobControllerSP mob)
    {
        mob.SwarmIndex = activeMobs.Count;
        activeMobs.Add(mob);
    }

    public void UnregisterMob(MobControllerSP mob)
    {
        int index = mob.SwarmIndex;
        if (index < 0 || index >= activeMobs.Count || activeMobs[index] != mob) return;
        int lastIndex = activeMobs.Count - 1;
        MobControllerSP lastMob = activeMobs[lastIndex];
        activeMobs[index] = lastMob;
        lastMob.SwarmIndex = index;
        activeMobs.RemoveAt(lastIndex);
        mob.SwarmIndex = -1;
    }

    public void OnUpdate()
    {
        if (activeMobs.Count == 0 || _player == null) return;

        if (PerformanceMonitor.Instance != null)
        {
            PerformanceMonitor.Instance.StartLogicTimer();
        }
        
        SpatialGrid.Instance.ClearGrid();
        Vector2 playerPos = _player.position;
        float dt = Time.deltaTime;
        
        for (int i = 0; i < activeMobs.Count; i++)
        {
            MobControllerSP mob = activeMobs[i];
            mob.MovementSP.CurrentPos = mob.transform.position; 
            SpatialGrid.Instance.Register(mob);
        }

        for (int i = 0; i < activeMobs.Count; i++)
        {
            MobControllerSP mob = activeMobs[i];
            MobMovementSP moveComp = mob.MovementSP;

            Vector3 myPos = moveComp.CurrentPos;
            
            
            mob.TickMelee(dt);

            moveComp.ForceToApply /= 1.2f;
            if (moveComp.ForceToApply.sqrMagnitude <= 0.01f) moveComp.ForceToApply = Vector2.zero;

            moveComp.SeparationForce = Vector2.zero;

            SpatialGrid.Instance.GetNearbyEntities(myPos, _radius, ref mob.NearbyNeighbors);

            for (int j = 0; j < mob.NearbyNeighbors.Count; j++)
            {
                MobControllerSP neighbor = mob.NearbyNeighbors[j];
                if (neighbor == mob) continue;

                Vector2 offset = (Vector2)myPos - (Vector2)neighbor.MovementSP.CurrentPos;
                float sqrDist = offset.sqrMagnitude;

                if (sqrDist == 0)
                {
                    offset = new Vector2(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f));
                    sqrDist = offset.sqrMagnitude;
                }

                if (sqrDist > 0 && sqrDist < _sqrRadius)
                {
                    float dist = Mathf.Sqrt(sqrDist);
                    moveComp.SeparationForce += (offset / dist) * ((_radius - dist) * _sepWeight);
                }
            }

            float sqrDistToPlayer = (playerPos - (Vector2)myPos).sqrMagnitude;
            float attackRange = mob.MobData.attackRange; 
            float sqrAttackRange = attackRange * attackRange;
            
            Vector2 walkVelocity = Vector2.zero;
            
            if (sqrDistToPlayer <= sqrAttackRange)
            {
                mob.TryMeleeAttack(_playerController, myPos);
            }
            else
            {
                Vector2 direction = (playerPos - (Vector2)myPos).normalized;
                walkVelocity = direction * mob.MobData.mobSpeed;
            }

            Vector2 finalVelocity = walkVelocity + moveComp.ForceToApply + moveComp.SeparationForce;

            Vector3 finalPos = myPos + (Vector3)finalVelocity * Time.deltaTime;
            mob.transform.position = finalPos;
            mob.MovementSP.CurrentPos = finalPos;
            
            if (PerformanceMonitor.Instance != null)
            {
                PerformanceMonitor.Instance.StopLogicTimer(activeMobs.Count);
            }
            
        }
    }
}