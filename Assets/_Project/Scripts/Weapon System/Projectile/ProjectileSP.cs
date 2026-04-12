using System.Collections.Generic;
using UnityEngine;

public class ProjectileSP : MonoBehaviour
{
    [Header("Settings")]
    public float HitboxRadius = 0.5f;
    [SerializeField] private GameObject _hitEffect;

    [HideInInspector] public float SqrRadius;
    [HideInInspector] public float Damage;
    [HideInInspector] public int PierceCount;
    [HideInInspector] public float KnockbackForce;
    [HideInInspector] public float Speed;
    [HideInInspector] public Vector3 Direction;
    [HideInInspector] public Vector3 CurrentPos;
    
    [HideInInspector] public float LifetimeTimer;
    [HideInInspector] public int ProjIndex = -1;

    private bool _isRegistered = false;

    public HashSet<MobControllerSP> HitTargets = new HashSet<MobControllerSP>();
    public List<MobControllerSP> NearbyEnemies = new List<MobControllerSP>();

    void Awake()
    {
        SqrRadius = HitboxRadius * HitboxRadius;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        SqrRadius = HitboxRadius * HitboxRadius;
    }
#endif

    void OnEnable()
    {
        if (ProjectileManager.Instance != null && !_isRegistered)
        {
            ProjectileManager.Instance.SpawnProjectile(this);
            _isRegistered = true;
        }
    }

    void OnDisable()
    {
        if (ProjectileManager.Instance != null && _isRegistered)
        {
            ProjectileManager.Instance.DespawnProjectile(this);
            _isRegistered = false;
        }
    }

    public void Setup(float dmg, float speed, float lifetime, int pierce, float knockback, Vector3 direction, Vector3  startPos)
    {
        Damage = dmg;
        Speed = speed;
        PierceCount = pierce;
        KnockbackForce = knockback;
        Direction = direction;
        LifetimeTimer = lifetime;
        CurrentPos = startPos;

        HitTargets.Clear();
    }

    public void Explode(Vector3 hitPos)
    {
        if (_hitEffect != null)
        {
            PoolManager.Instance.Spawn(_hitEffect, hitPos, Quaternion.identity);
        }
        PoolManager.Instance.Despawn(gameObject);
    }

    public void Die()
    {
        PoolManager.Instance.Despawn(gameObject);
    }
}
