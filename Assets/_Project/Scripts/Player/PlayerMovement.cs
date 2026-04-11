using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
        
        if (_forceToApply.sqrMagnitude <= 0.01f)
        {
            _forceToApply = Vector2.zero;
        }

        Vector2 moveForce = (_playerInput * _moveSpeed) + _forceToApply;
        _rb.velocity = moveForce;
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