using UnityEngine;

public class MobMovement : MonoBehaviour, IFixedUpdater
{
    [SerializeField] private Rigidbody2D _rb;
    private MobData _data;
    private Transform _target;
    private Vector2 _forceToApply;

    [SerializeField] private float _forceDamping = 1.2f;
    
    public void Init(MobData data, Transform target)
    {
        _data = data;
        _target = target;
    }
    void OnEnable()
    {
        UpdateManager.Instance.OnAssignFixedUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.OnUnassignFixedUpdater(this);
        }
    }
    
    public void OnFixedUpdate()
    {
        if (_target == null) return;
        Move();
    }

    private void Move()
    {
        _forceToApply = Vector2.Lerp(_forceToApply, Vector2.zero, _forceDamping * Time.fixedDeltaTime);

        if (_forceToApply.sqrMagnitude <= 0.01f) 
        {
            _forceToApply = Vector2.zero;
        }

        Vector2 walkVelocity = Vector2.zero;
        if (_target != null)
        {
            Vector2 offset = (Vector2)_target.position - (Vector2)transform.position;
        
            if (offset.sqrMagnitude > 0.01f) 
            {
                Vector2 direction = offset.normalized;
                walkVelocity = direction * _data.mobSpeed;
            }
        }

        _rb.velocity = walkVelocity + _forceToApply;
    }

    public void TakeKnockback(Vector2 knockback)
    {
        _forceToApply += knockback * _data.knockbackResistance;
    }
}