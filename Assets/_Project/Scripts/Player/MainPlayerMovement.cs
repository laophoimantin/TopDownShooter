using UnityEngine;

public class MainPlayerMovement : MonoBehaviour , IDamageable
{
    [Header("Animator")] private Animator anim;

    //Move
    [Header("Move")] public float moveSpeed = 1;
    private Rigidbody2D rb;
    private Vector2 playerInput;
    [HideInInspector] public Vector2 forceToApply;
    [SerializeField] private float forceDamping = 1.2f;
    private SpriteRenderer sprite;

    [Header("Player's Stats")] private PlayerStats playerStats;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        GetInput();
        Flip();
    }

    private void Flip()
    {
        if (rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 moveForce = playerInput * moveSpeed;
        moveForce += forceToApply;
        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
        {
            forceToApply = Vector2.zero;
        }

        rb.velocity = moveForce;
    }


    private void GetInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        playerInput = new Vector2(horizontalInput, verticalInput).normalized;
        if (playerInput != Vector2.zero)
        {
            anim.SetBool("IsWalking", true);
        }
        else
        {
            anim.SetBool("IsWalking", false);
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 knock = (transform.position - collision.transform.position).normalized;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

		Vector2 knock = (transform.position - collision.transform.position).normalized;
       
        if ((collision.CompareTag("Mob") || collision.CompareTag("MobBullet")) && playerStats.playerCurrentHealth > 0 &&
         playerStats.invincibilityTimer <= 0)
        {
        }
    }

    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
            playerStats.GetHit();
            forceToApply += new Vector2(knockbackVector.x * 20f, knockbackVector.y * 20f);
    }
}