using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class SwarmManager : Singleton<SwarmManager>, IUpdater
{
    public List<MobController> activeMobs = new List<MobController>();
    [SerializeField] private Transform _player;

    // private TransformAccessArray _transformAccessArray;
    // private NativeArray<Vector3> _separationForces;

    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.OnUnassignUpdater(this);
    }

    public void OnUpdate()
    {
        if (activeMobs.Count == 0) return;

        SpatialGrid.Instance.ClearGrid();

        for (int i = 0; i < activeMobs.Count; i++)
        {
            MobController mob = activeMobs[i];
            MobMovementSP moveComp = mob.MovementSP;

            moveComp._forceToApply /= 1.2f;
            if (moveComp._forceToApply.sqrMagnitude <= 0.01f) moveComp._forceToApply = Vector2.zero;

            moveComp._separationForce = Vector2.zero;
            SpatialGrid.Instance.GetNearbyEntities(mob.transform.position, 1.5f, ref mob.NearbyNeighbors);
        }

        for (int i = 0; i < activeMobs.Count; i++)
        {
            MobController mob = activeMobs[i];
            MobMovementSP moveComp = mob.MovementSP;

            Vector2 direction = (_player.position - mob.transform.position).normalized;
            Vector2 walkVelocity = direction * mob.MobData.mobSpeed;

            Vector2 finalVelocity = walkVelocity + moveComp._forceToApply + moveComp._separationForce;

            mob.transform.position += (Vector3)finalVelocity * Time.deltaTime;

            SpatialGrid.Instance.Register(mob);
        }
    }
}