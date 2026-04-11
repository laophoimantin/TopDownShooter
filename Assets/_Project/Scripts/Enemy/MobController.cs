using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour, IDamageable, IUpdater
{
    [Header("Data")]
    [SerializeField] private MobData _mobData;
    public MobData MobData => _mobData;

    [Header("References")]
    [SerializeField] private MobHealth _health;
    [SerializeField] private MobMovement _movement;
    [SerializeField] private MobVisuals _visuals;


    [Header("Attack")]
    [SerializeField] private MobMelee _meleeAttacker;
    private Transform _targetPlayer;
    private EnemySpawner _mySpawner;

    [Header("Spatial Partitioning")]
    [SerializeField] private bool _UseSP;
    [SerializeField] private MobMovementSP _movementSP;
    public MobMovementSP MovementSP => _movementSP;

    [HideInInspector] public List<MobController> NearbyNeighbors = new();


    public void Init(Transform targetPlayer, EnemySpawner spawner)
    {
        _targetPlayer = targetPlayer;
        _mySpawner = spawner;

        _health.Init(_mobData, _mySpawner);
        _movement.Init(_mobData, _targetPlayer);

        if (_visuals != null)
            _visuals.Init(_mobData, _targetPlayer);

        if (_meleeAttacker != null)
            _meleeAttacker.Init(_mobData);
    }

    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.OnUnassignUpdater(this);
        }
    }

    public void OnUpdate()
    {
        // if (_targetPlayer != null)
        // {
        //     if (Vector2.Distance(transform.position, _targetPlayer.position) >= _mobData.despawnDistance)
        //     {
        //         ReturnToSpawner();
        //     }
        // }
    }

    private void ReturnToSpawner()
    {
        if (_mySpawner != null)
        {
            transform.position = _mySpawner.GetRandomAvailableSpawnPoint();
        }
    }

    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
        _health.DecreaseHealth(dmg);

        if (_mobData.blood != null)
            PoolManager.Instance.Spawn(_mobData.blood, transform.position, Quaternion.identity);

        if (!_UseSP)
            _movement.TakeKnockback(knockbackVector);
        else
            _movementSP.TakeKnockback(knockbackVector);
    }
}