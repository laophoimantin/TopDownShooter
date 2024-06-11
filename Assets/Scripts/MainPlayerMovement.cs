using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    private PlayerStats playerStats;

    //Move
    [Header("Move")]
    public float moveSpeed = 1;
    private Rigidbody2D rb;
    private Vector2 playerInput;
    public Vector2 forceToApply;
    [SerializeField] private float forceDamping = 1.2f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerStats = rb.GetComponent<PlayerStats>();
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

}
