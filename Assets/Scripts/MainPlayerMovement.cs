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
    public Vector2 forceToApply;
    [SerializeField] private float forceDamping = 1.2f;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        GetInput();

        if (rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    // Used for physics calculations(No Idea)
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
}
