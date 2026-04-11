using UnityEngine;

public class Projectile : MonoBehaviour, IUpdater
{
    private float _damage;
    private int _pierceCount;
    private float _knockbackForce;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject _hitEffect;

    private float _lifetimeTimer;
    private bool _isDead;

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
        if (_isDead) return;

        _lifetimeTimer -= Time.deltaTime;
        if (_lifetimeTimer <= 0)
        {
            Die();
        }
    }

    public void Setup(float dmg, float speed, float lifetime, int pierce, float knockback)
    {
        _damage = dmg;
        _pierceCount = pierce;
        _knockbackForce = knockback;

        _lifetimeTimer = lifetime;
        _isDead = false;

        _rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDead) return;

        if (collision.TryGetComponent(out IDamageable victim))
        {
            Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
            Vector2 finalKnockback = knockbackDir * _knockbackForce;

            victim.TakeDamage(_damage, finalKnockback);

            if (_pierceCount > 0)
            {
                _pierceCount--;
            }
            else
            {
                Explode();
            }
        }
        else if (collision.CompareTag("Wall"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        _isDead = true;
        _rb.velocity = Vector2.zero;

        if (_hitEffect != null)
        {
            PoolManager.Instance.Spawn(_hitEffect, transform.position, Quaternion.identity);
        }

        PoolManager.Instance.Despawn(gameObject);
    }

    private void Die()
    {
        _isDead = true;
        _rb.velocity = Vector2.zero;
        PoolManager.Instance.Despawn(gameObject);
    }
}