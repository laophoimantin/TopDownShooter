using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

public class SwarmManager : Singleton<SwarmManager>, IUpdater
{
    [SerializeField] private PlayerController _playerController;

    // -------------------------------------------------------
    public List<MobControllerSP> activeMobs = new();

    [Header("Batching Optimization")]
    [Range(1, 50)] public int totalBatches = 10;
    private int _currentBatchIndex = 0;

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
        int totalMobs = activeMobs.Count;
        if (totalMobs == 0 || _playerController == null) return;

        PerformanceMonitor.Instance?.StartLogicTimer();

        CalculateBatchIndices(totalMobs, out int startIndex, out int endIndex);
        RefreshSpatialGrid(totalMobs);

        float dt = Time.deltaTime * totalBatches;
        Vector2 playerPos = _playerController.transform.position;

        ProcessMobBatch(startIndex, endIndex, dt, playerPos);

        PerformanceMonitor.Instance?.StopLogicTimer(totalMobs);
    }

    private void CalculateBatchIndices(int totalMobs, out int startIndex, out int endIndex)
    {
        int mobsPerBatch = Mathf.CeilToInt((float)totalMobs / totalBatches);
        startIndex = _currentBatchIndex * mobsPerBatch;
        endIndex = Mathf.Min(startIndex + mobsPerBatch, totalMobs);
        _currentBatchIndex = (_currentBatchIndex + 1) % totalBatches;
    }

    private void RefreshSpatialGrid(int totalMobs)
    {
        SpatialGrid.Instance.ClearGrid();
        for (int i = 0; i < totalMobs; i++)
        {
            SpatialGrid.Instance.Register(activeMobs[i]);
        }
    }

    // Divide the whole list of enemies into small batches
    private void ProcessMobBatch(int startIndex, int endIndex, float dt, Vector2 playerPos)
    {
        for (int i = startIndex; i < endIndex; i++)
        {
            MobControllerSP mob = activeMobs[i];
            MobMovementSP moveComp = mob.MovementSP;
            Vector3 myPos = moveComp.CurrentPos;

            mob.Visuals.SetFacingLeft(playerPos.x < myPos.x);

            mob.TickMelee(dt);

            ApplyKnockbackFriction(moveComp);

            moveComp.SeparationForce = CalculateSeparationForce(mob, myPos);

            Vector2 walkVelocity = CalculateWalkVelocity(mob, myPos, playerPos);

            Vector2 finalVelocity = walkVelocity + moveComp.ForceToApply + moveComp.SeparationForce;
            Vector3 finalPos = myPos + (Vector3)finalVelocity * dt;

            mob.transform.position = finalPos;
            moveComp.CurrentPos = finalPos;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ApplyKnockbackFriction(MobMovementSP moveComp)
    {
        moveComp.ForceToApply /= Mathf.Pow(1.2f, totalBatches);
        if (moveComp.ForceToApply.sqrMagnitude <= 0.01f)
        {
            moveComp.ForceToApply = Vector2.zero;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 CalculateSeparationForce(MobControllerSP mob, Vector3 myPos)
    {
        float currentRadius = mob.MobData.separationRadius;
        float currentSqrRadius = currentRadius * currentRadius;
        float currentWeight = mob.MobData.separationWeight;
        float myMass = mob.MobData.mass; 

        Vector2 separationForce = Vector2.zero;
        SpatialGrid.Instance.GetNearbyEntities(myPos, currentRadius, ref mob.NearbyNeighbors);

        int count = mob.NearbyNeighbors.Count;
        for (int j = 0; j < count; j++)
        {
            MobControllerSP neighbor = mob.NearbyNeighbors[j];
            if (neighbor == mob) continue;

            Vector2 offset = (Vector2)myPos - (Vector2)neighbor.MovementSP.CurrentPos;
            float sqrDist = offset.sqrMagnitude;

            if (sqrDist == 0)
            {
                offset = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                sqrDist = offset.sqrMagnitude;
            }

            if (sqrDist > 0 && sqrDist < currentSqrRadius)
            {
                float dist = Mathf.Sqrt(sqrDist);
                float neighborMass = neighbor.MobData.mass;
                float massRatio = neighborMass / myMass;

                separationForce += (offset / dist) * ((currentRadius - dist) * currentWeight * massRatio);
            }
        }

        return separationForce;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 CalculateWalkVelocity(MobControllerSP mob, Vector3 myPos, Vector2 playerPos)
    {
        float sqrDistToPlayer = (playerPos - (Vector2)myPos).sqrMagnitude;
        float attackRange = mob.MobData.attackRange;

        if (sqrDistToPlayer <= (attackRange * attackRange))
        {
            mob.TryMeleeAttack(_playerController, myPos);
            return Vector2.zero;
        }

        Vector2 direction = (playerPos - (Vector2)myPos).normalized;
        return direction * mob.MobData.mobSpeed;
    }
}

// public void OnUpdate()
// {
//     int totalMobs = activeMobs.Count;
//     if (totalMobs == 0 || _playerController == null) return;
//
//     if (PerformanceMonitor.Instance != null)
//     {
//         PerformanceMonitor.Instance.StartLogicTimer();
//     }
//
//     int mobsPerBatch = Mathf.CeilToInt((float)totalMobs / totalBatches);
//     int startIndex = _currentBatchIndex * mobsPerBatch;
//     int endIndex = Mathf.Min(startIndex + mobsPerBatch, totalMobs);
//     _currentBatchIndex = (_currentBatchIndex + 1) % totalBatches;
//
//     // ==========================================
//
//     SpatialGrid.Instance.ClearGrid();
//     Vector2 playerPos = _playerController.transform.position;
//
//     for (int i = 0; i < totalMobs; i++)
//     {
//         MobControllerSP mob = activeMobs[i];
//         SpatialGrid.Instance.Register(mob);
//     }
//
//     float dt = Time.deltaTime * totalBatches;
//
//     for (int i = startIndex; i < endIndex; i++)
//     {
//         MobControllerSP mob = activeMobs[i];
//         MobMovementSP moveComp = mob.MovementSP;
//
//         Vector3 myPos = moveComp.CurrentPos;
//
//         bool isFacingLeft = playerPos.x < myPos.x;
//         mob.Visuals.SetFacingLeft(isFacingLeft);
//
//         float currentRadius = mob.MobData.separationRadius;
//         float currentSqrRadius = currentRadius * currentRadius;
//         float currentWeight = mob.MobData.separationWeight;
//
//         mob.TickMelee(dt);
//
//         moveComp.ForceToApply /= Mathf.Pow(1.2f, totalBatches);
//         if (moveComp.ForceToApply.sqrMagnitude <= 0.01f) moveComp.ForceToApply = Vector2.zero;
//         moveComp.SeparationForce = Vector2.zero;
//
//         SpatialGrid.Instance.GetNearbyEntities(myPos, currentRadius, ref mob.NearbyNeighbors);
//
//         for (int j = 0; j < mob.NearbyNeighbors.Count; j++)
//         {
//             MobControllerSP neighbor = mob.NearbyNeighbors[j];
//             if (neighbor == mob) continue;
//
//             Vector2 offset = (Vector2)myPos - (Vector2)neighbor.MovementSP.CurrentPos;
//             float sqrDist = offset.sqrMagnitude;
//
//             if (sqrDist == 0)
//             {
//                 offset = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
//                 sqrDist = offset.sqrMagnitude;
//             }
//
//             if (sqrDist > 0 && sqrDist < currentSqrRadius)
//             {
//                 float dist = Mathf.Sqrt(sqrDist);
//                 moveComp.SeparationForce += (offset / dist) * ((currentRadius - dist) * currentWeight);
//             }
//         }
//
//         float sqrDistToPlayer = (playerPos - (Vector2)myPos).sqrMagnitude;
//         float attackRange = mob.MobData.attackRange;
//         float sqrAttackRange = attackRange * attackRange;
//
//         Vector2 walkVelocity = Vector2.zero;
//
//         if (sqrDistToPlayer <= sqrAttackRange)
//         {
//             mob.TryMeleeAttack(_playerController, myPos);
//         }
//         else
//         {
//             Vector2 direction = (playerPos - (Vector2)myPos).normalized;
//             walkVelocity = direction * mob.MobData.mobSpeed;
//         }
//
//         Vector2 finalVelocity = walkVelocity + moveComp.ForceToApply + moveComp.SeparationForce;
//
//         Vector3 finalPos = myPos + (Vector3)finalVelocity * dt;
//
//         mob.transform.position = finalPos;
//         mob.MovementSP.CurrentPos = finalPos;
//     }
//
//     if (PerformanceMonitor.Instance != null)
//     {
//         PerformanceMonitor.Instance.StopLogicTimer(totalMobs);
//     }
// }
