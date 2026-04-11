using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour, IDamageable
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
    private bool _isRegistered = false;
    [SerializeField] private MobMovementSP _movementSP;
    public MobMovementSP MovementSP => _movementSP;
    [HideInInspector] public int SwarmIndex = -1;
    [HideInInspector] public List<MobController> NearbyNeighbors = new();

    void OnEnable()
    {
        if (_movementSP !=null && SwarmManager.Instance != null && !_isRegistered)
        {
            SwarmManager.Instance.RegisterMob(this);
            _isRegistered = true;
        }
    }
    void OnDisable()
    {
        if (_movementSP !=null && SwarmManager.Instance != null && _isRegistered)
        {
            SwarmManager.Instance.UnregisterMob(this);
            _isRegistered = false;
        }
    }
    
    public void Init(Transform targetPlayer, EnemySpawner spawner)
    {
        _targetPlayer = targetPlayer;
        _mySpawner = spawner;

        _health.Init(_mobData, _mySpawner);
        if (_movement != null)
        {
            _movement.Init(_mobData, _targetPlayer);
        }
        else
        {
            _movementSP.Init(_mobData);
        }

        if (_visuals != null)
            _visuals.Init(_mobData, _targetPlayer);

        if (_meleeAttacker != null)
            _meleeAttacker.Init(_mobData);
    }


    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
        _health.DecreaseHealth(dmg);

        if (_mobData.blood != null)
            PoolManager.Instance.Spawn(_mobData.blood, transform.position, Quaternion.identity);

        if (_movement != null)
            _movement.TakeKnockback(knockbackVector);
        else
            _movementSP.TakeKnockback(knockbackVector);
    }
    
    private void ReturnToSpawner()
    {
        if (_mySpawner != null)
        {
            transform.position = _mySpawner.GetRandomAvailableSpawnPoint();
        }
    }
}