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

    public void Init(Transform targetPlayer, Vector3 startPos)
    {
        _targetPlayer = targetPlayer;

        _health.Init(_mobData);
        if (_movement != null)
            _movement.Init(_mobData, _targetPlayer);

        if (_visuals != null)
            _visuals.Init(_mobData, _targetPlayer);

        if (_meleeAttacker != null)
            _meleeAttacker.Init(_mobData);
    }


    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
        Debug.Log(dmg);
        _health.DecreaseHealth(dmg);

        if (_mobData.blood != null)
            PoolManager.Instance.Spawn(_mobData.blood, transform.position, Quaternion.identity);

        if (_movement != null)
            _movement.TakeKnockback(knockbackVector);
    }
}