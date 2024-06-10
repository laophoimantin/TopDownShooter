using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    //Move
    [Header("Move")]
    public float moveSpeed = 1;
    private Rigidbody2D rb;
    private Vector2 playerInput;
    private Vector2 forceToApply;
    [SerializeField] private float forceDamping = 1.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        GetInput();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Mob"))
        {
            forceToApply += new Vector2(-20, 0);
        }
    }
}
