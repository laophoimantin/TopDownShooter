using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobControllerSP : MonoBehaviour, IDamageable
{
    private Transform _targetPlayer;
    
    // ------------------------------------------------------------
    [Header("Data")]
    [SerializeField] private MobData _mobData;
    public MobData MobData => _mobData;
    
    // ------------------------------------------------------------
    [Header("References")]
    [SerializeField] private MobHealth _health;
    [SerializeField] private MobMovementSP _movementSP;
    [SerializeField] private MobMeleeSP _melee;
    [SerializeField] private MobVisuals _visuals;
    
    public MobMovementSP MovementSP => _movementSP;

    // ------------------------------------------------------------
    [Header("Spatial Partitioning")]
    private bool _isRegistered = false;
    [HideInInspector] public int SwarmIndex = -1;
    [HideInInspector] public List<MobControllerSP> NearbyNeighbors = new();

    // =================================================================================================================
    void OnEnable()
    {
        if (_movementSP != null && SwarmManager.Instance != null && !_isRegistered)
        {
            SwarmManager.Instance.RegisterMob(this);
            _isRegistered = true;
        }
    }

    void OnDisable()
    {
        if (_movementSP != null && SwarmManager.Instance != null && _isRegistered)
        {
            SwarmManager.Instance.UnregisterMob(this);
            _isRegistered = false;
        }
    }

    public void Init(Transform targetPlayer, Vector3 startPos)
    {
        _targetPlayer = targetPlayer;
        
        _health.Init(_mobData);
        _movementSP.Init(_mobData, startPos);
        if (_visuals != null)
            _visuals.Init(_mobData, _targetPlayer);
        if (_melee != null)
            _melee.Init(_mobData);
    }
    
    // ============================================================
    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
        _health.DecreaseHealth(dmg);

        if (_mobData.blood != null)
            PoolManager.Instance.Spawn(_mobData.blood, transform.position, Quaternion.identity);
        _movementSP.TakeKnockback(knockbackVector);
    }
    
    public void TickMelee(float dt)
    {
        if (_melee != null) 
        {
            _melee.TickCooldown(dt);
        }
    }

    public bool TryMeleeAttack(PlayerController player, Vector3 myPos)
    {
        if (_melee != null) 
        {
            return _melee.TryAttack(player, myPos);
        }
        return false;
    }
}