using JetBrains.Rider.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerMovement : MonoBehaviour
{
    //Move
    [Header("Move")]
    [SerializeField] private float speed = 1;
    private Rigidbody2D rb;
    private Vector2 playerInput;

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
        rb.velocity = playerInput * speed;
    }

    private void GetInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        playerInput = new Vector2(horizontalInput, verticalInput).normalized;
    }
}
