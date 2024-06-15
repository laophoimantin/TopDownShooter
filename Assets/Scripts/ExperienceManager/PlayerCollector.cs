using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private PlayerStats player;
    private CircleCollider2D playerCollector;
    [SerializeField] float pullSpeed = 300f;


    private void Start()
    {
        player = GetComponent<PlayerStats>();
        playerCollector = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out ICollectible collectible))
        {
            //Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            //Vector2 forceDirection = (transform.position - collision.transform.position).normalized;
            //rb.AddForce(forceDirection * pullSpeed);
            collision.transform.position = Vector2.MoveTowards(collision.transform.position, transform.position, pullSpeed);
            //collectible.Collect();

        }
    }
}
