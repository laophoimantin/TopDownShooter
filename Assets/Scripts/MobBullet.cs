using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobBullet : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float mobBulletLifetime = 5f;
    private bool playerFound = false;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();

        if (!playerFound)
        {
            Destroy(gameObject, mobBulletLifetime);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("playerFound", true);
            playerFound = true;
            rb.velocity = Vector2.zero;
            Destroy(circleCollider);
            Destroy(gameObject, 1);
            Debug.Log("Player hit");
        }
    }
}
