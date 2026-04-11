using UnityEngine;

public class MobController : MonoBehaviour, IDamageable, IUpdater, IFixedUpdater
{
    [Header("Data")]
    [SerializeField] private MobData _mobData;

    [Header("References")]
    [SerializeField] private MobHealth _health;
    [SerializeField] private MobMovement _movement;
    [SerializeField] private MobVisuals _visuals;
   

    [Header("Attack")]
    [SerializeField] private MobMelee _meleeAttacker;
    [SerializeField] private MobShooter _shooter;
    private Transform _targetPlayer;
    private EnemySpawner _mySpawner;

    public void Init(Transform targetPlayer, EnemySpawner spawner)
    {
        _targetPlayer = targetPlayer;
        _mySpawner = spawner;

        _health.Init(_mobData, _mySpawner);
        _movement.Init(_mobData, _targetPlayer);
        _visuals.Init(_mobData, _targetPlayer);

        if (_meleeAttacker != null)
            _meleeAttacker.Init(_mobData);

        if (_shooter != null)
            _shooter.Init(_mobData, _targetPlayer);
    }

    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
        UpdateManager.Instance.OnAssignFixedUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.OnUnassignUpdater(this);
            UpdateManager.Instance.OnUnassignFixedUpdater(this);
        }
    }

    public void OnUpdate()
    {
        if (_targetPlayer != null)
        {
            if (Vector2.Distance(transform.position, _targetPlayer.position) >= _mobData.despawnDistance)
            {
                ReturnToSpawner();
            }
        }

        _health.OnUpdate();
        _movement.OnUpdate();
        _visuals.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        _health.OnFixedUpdate();
        _movement.OnFixedUpdate();
        _visuals.OnFixedUpdate();
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
        _movement.TakeKnockback(knockbackVector);
    }
}