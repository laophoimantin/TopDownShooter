using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    [Header("Animator")]
    private Animator anim;

    //Move
    [Header("Move")]
    public float moveSpeed = 1;
    private Rigidbody2D rb;
    private Vector2 playerInput;
    [HideInInspector] public Vector2 forceToApply;
    [SerializeField] private float forceDamping = 1.2f;
    private SpriteRenderer sprite;

    [Header("Player's Stats")]
    private PlayerStats playerStats;

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
        Hit(collision.collider);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hit(collision);
    }

    private void Hit(Collider2D collider)
    {
        if ((collider.CompareTag("Mob") || collider.CompareTag("MobBullet")) && playerStats.playerCurrentHealth > 0 && playerStats.invincibilityTimer <= 0)
        {
            playerStats.GetHit();

            Vector2 knockbackDirection = (transform.position - collider.transform.position).normalized;
            forceToApply += new Vector2(knockbackDirection.x * 20f, knockbackDirection.y * 20f);
        }
    }
}
