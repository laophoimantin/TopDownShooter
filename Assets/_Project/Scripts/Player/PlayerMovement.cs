using UnityEngine;

public class PlayerMovement : MonoBehaviour, IUpdater, IFixedUpdater
{
    [Header("References")]
    [SerializeField] private Rigidbody2D _rb;

    [Header("Movement Stats")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _maxSpeed = 15f;

    [Header("Knockback Stats")]
    [SerializeField] private float _fixedKnockbackForce = 20f;
    [SerializeField] private float _forceDamping = 1.2f;

    private Vector2 _playerInput;
    private Vector2 _forceToApply;

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
        GetInput();
    }

    public void OnFixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _forceToApply = Vector2.Lerp(_forceToApply, Vector2.zero, _forceDamping * Time.fixedDeltaTime);

        if (_forceToApply.sqrMagnitude <= 2f)
        {
            _forceToApply = Vector2.zero;
        }

        if (_forceToApply != Vector2.zero)
        {
            _rb.velocity = _forceToApply;
        }
        else 
        {
            _rb.velocity = _playerInput * _moveSpeed;
        }
    }

    public void StopMovement()
    {
        _rb.velocity = Vector2.zero;
    }
    
    private void GetInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        _playerInput = new Vector2(horizontalInput, verticalInput).normalized;
    }

    public void TakeKnockback(Vector2 knockbackDirection)
    {
        _forceToApply = knockbackDirection.normalized * _fixedKnockbackForce;
    }

    public void AddSpeed(float speed)
    {
        _moveSpeed += speed;
        if (_moveSpeed > _maxSpeed)
        {
            _moveSpeed = _maxSpeed;
        }
    }
}