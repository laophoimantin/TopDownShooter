using UnityEngine;

public class MobMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    private MobData _data;
    private Transform _target;
    private Vector2 _forceToApply;

    public void Init(MobData data, Transform target)
    {
        _data = data;
        _target = target;
    }

    public void OnUpdate()
    {
        
    }
    
    public void OnFixedUpdate()
    {
        if (_target == null) return;
        Move();
    }

    private void Move()
    {
        _forceToApply /= 1.2f; // Damping
        if (_forceToApply.sqrMagnitude <= 0.01f) _forceToApply = Vector2.zero;

        Vector2 direction = (_target.position - transform.position).normalized;
        Vector2 walkVelocity = direction * _data.mobSpeed;

        _rb.velocity = walkVelocity + _forceToApply;
    }

    public void TakeKnockback(Vector2 knockback)
    {
        _forceToApply += knockback * _data.knockbackResistance;
    }
}