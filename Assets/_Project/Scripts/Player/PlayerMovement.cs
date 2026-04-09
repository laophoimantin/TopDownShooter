using UnityEngine;

public class PlayerMovement : MonoBehaviour 
{

    [Header("References")] 
    [SerializeField] private PlayerHealth _playerStats;
    [SerializeField] private Rigidbody2D _rb;

    [Header("Player's Stats")] 
    private Vector2 _playerInput;
    [SerializeField] private float _moveSpeed = 1;
    private Vector2 _forceToApply;
    [SerializeField] private float _forceDamping = 1.2f;


    void Start()
    {
    }

    void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveForce = _playerInput * _moveSpeed;
        moveForce += _forceToApply;
        _forceToApply /= _forceDamping;
        if (Mathf.Abs(_forceToApply.x) <= 0.01f && Mathf.Abs(_forceToApply.y) <= 0.01f)
        {
            _forceToApply = Vector2.zero;
        }

        _rb.velocity = moveForce;
    }


    private void GetInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        _playerInput = new Vector2(horizontalInput, verticalInput).normalized;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 knock = (transform.position - collision.transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

		Vector2 knock = (transform.position - collision.transform.position).normalized;
       
        if ((collision.CompareTag("Mob") || collision.CompareTag("MobBullet")) && _playerStats.CurrentHealth > 0 &&
         _playerStats.invincibilityTimer <= 0)
        {

        }
    }

    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
            _playerStats.GetHit();
            _forceToApply += new Vector2(knockbackVector.x * 20f, knockbackVector.y * 20f);
    }
}