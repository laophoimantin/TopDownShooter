using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModControllerSP : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private MobData _mobData;
    public MobData MobData => _mobData;

    [Header("References")]
    [SerializeField] private MobHealth _health;
    [SerializeField] private MobMovementSP _movementSP;
    [SerializeField] private MobVisuals _visuals;
    public MobMovementSP MovementSP => _movementSP;


    [Header("Attack")]
    [SerializeField] private MobMelee _meleeAttacker;
    private Transform _targetPlayer;

    [Header("Spatial Partitioning")]
    private bool _isRegistered = false;
    [HideInInspector] public int SwarmIndex = -1;
    [HideInInspector] public List<MobController> NearbyNeighbors = new();

    void OnEnable()
    {
        if (_movementSP !=null && SwarmManager.Instance != null && !_isRegistered)
        {
            //SwarmManager.Instance.RegisterMob(this);
            _isRegistered = true;
        }
    }
    void OnDisable()
    {
        if (_movementSP !=null && SwarmManager.Instance != null && _isRegistered)
        {
          //  SwarmManager.Instance.UnregisterMob(this);
            _isRegistered = false;
        }
    }
    
    // public void Init(Transform targetPlayer, Vector3 startPos)
    // {
    //     _targetPlayer = targetPlayer;
    //
    //     _health.Init(_mobData);
    //     if (_movement != null)
    //     {
    //         _movement.Init(_mobData, _targetPlayer);
    //     }
    //     else
    //     {
    //         _movementSP.Init(_mobData, startPos);
    //     }
    //
    //     if (_visuals != null)
    //         _visuals.Init(_mobData, _targetPlayer);
    //
    //     if (_meleeAttacker != null)
    //         _meleeAttacker.Init(_mobData);
    // }
    //
    //
    // public void TakeDamage(float dmg, Vector2 knockbackVector)
    // {
    //     Debug.Log(dmg);
    //     _health.DecreaseHealth(dmg);
    //
    //     if (_mobData.blood != null)
    //         PoolManager.Instance.Spawn(_mobData.blood, transform.position, Quaternion.identity);
    //
    //     if (_movement != null)
    //         _movement.TakeKnockback(knockbackVector);
    //     else
    //         _movementSP.TakeKnockback(knockbackVector);
    // }
}