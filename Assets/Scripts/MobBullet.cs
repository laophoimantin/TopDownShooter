using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBullet : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float bulletLifetime = 5f;
    private bool playerDetected = false;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (!playerDetected)
        {
            Destroy(gameObject, bulletLifetime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerFound", true);
            playerDetected = true;
            rb.velocity = Vector2.zero;
            Destroy(gameObject, 1);
        }
    }
}
