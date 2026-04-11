using UnityEngine;

public class MobMelee : MonoBehaviour
{
    private MobData _data;
    private float _attackTimer;

    public void Init(MobData data)
    {
        _data = data;
        _attackTimer = 0f;
    }

    void Update()
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (_attackTimer > 0) return;
        
        if (!other.CompareTag("Player"))
            return;
        if (other.TryGetComponent(out IDamageable victim))
        {
            Vector2 knockbackDir = (other.transform.position - transform.position).normalized;
            Vector2 knockback = knockbackDir * _data.KnockbackForce;

            victim.TakeDamage(1, knockback);
            _attackTimer = 0.5f;
        }
    }
}