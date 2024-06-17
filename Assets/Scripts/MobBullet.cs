using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBullet : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float mobBulletLifetime = 5f;
    private bool playerFound = false;
    private Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (!playerFound)
        {
            Destroy(gameObject, mobBulletLifetime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerFound", true);
            playerFound = true;
            rb.velocity = Vector2.zero;
            Destroy(gameObject, 1);
        }
    }
}
