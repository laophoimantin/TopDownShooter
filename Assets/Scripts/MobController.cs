using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobController : MonoBehaviour
{
    private GameObject player;
    private GunController gunController;
    private Rigidbody2D rb;

    //Movement
    [Header("Movement")]
    [SerializeField] private float mobSpeed = 0.5f;
    [SerializeField] private float mobHealth = 3f;
    [SerializeField] private bool ranger = false;

    private Vector2 forceToApply;
    [SerializeField] private float forceDamping = 1.2f;


    [SerializeField] private GameObject bulletPf;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float fireRate = 2f;
    private float fireRateTimer;
    void Start()
    {
        fireRateTimer = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
        gunController = player.GetComponent<GunController>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (mobHealth  > 0)
        {
            MoveTowardsPlayer();
            if (ranger)
            {
                fireRateTimer -= Time.deltaTime;
                Shooting();
            }
        }
        else if (mobHealth <= 0)
        {
            Debug.Log("MobDead, Destroy");
            //Destroy(gameObject);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        Vector2 newPosition = rb.position + direction * mobSpeed;
        newPosition += forceToApply;
        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
        {
            forceToApply = Vector2.zero;
        }
        rb.MovePosition(newPosition);
    }

    private void Shooting()
    {
        if (fireRateTimer <= 0)
        {
            GameObject bullet = Instantiate(bulletPf, transform.position, transform.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = (player.transform.position - transform.position).normalized;
            bulletRb.velocity = direction * bulletSpeed;
            fireRateTimer = fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && mobHealth > 0)
        {
            mobHealth -= gunController.damage;
            Debug.Log("getHit! Health remain: " + mobHealth );

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            forceToApply += new Vector2(knockbackDirection.x * 10f, knockbackDirection.y * 10f); 
            Debug.Log(knockbackDirection);
        }
    }
}
