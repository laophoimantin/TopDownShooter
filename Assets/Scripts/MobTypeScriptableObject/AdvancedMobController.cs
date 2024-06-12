using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedMobController : MonoBehaviour
{
    [Header("References")]
    private GameObject player;
    private AdvancedGunController gunController;
    private Rigidbody2D rb;

    [Header("Basic Data")]
    [SerializeField] private MobData MobDta;
    private float health;
    private float fireRateTimer;

    [Header("Knock Back")]
    private Vector2 forceToApply;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float forceDamping = 1.2f;

    [Header("Visual")]
    private SpriteRenderer visual;

    void Start()
    {
        fireRateTimer = 0f;
        player = GameObject.FindGameObjectWithTag("Player");
        gunController = player.GetComponent<AdvancedGunController>();
        rb = GetComponent<Rigidbody2D>();
        health = MobDta.mobHealth;

        visual = GetComponent<SpriteRenderer>();
        visual.sprite = MobDta.Image;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            MoveTowardsPlayer();
            if (MobDta.ranger)
            {
                fireRateTimer -= Time.deltaTime;
                Shooting();
            }
        }
        else if (health <= 0)
        {
            Destroy(gameObject);
        }

        if(player.transform.position.x < transform.position.x)
        {
            visual.flipX = true;
        }
        else if(player.transform.position.x > transform.position.x)
        {
            visual.flipX = false;
        }

    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        direction += forceToApply;
        forceToApply /= forceDamping;
        if (Mathf.Abs(forceToApply.x) <= 0.01f && Mathf.Abs(forceToApply.y) <= 0.01f)
        {
            forceToApply = Vector2.zero;
        }
        rb.velocity = direction;


    }

    private void Shooting()
    {
        if (fireRateTimer <= 0)
        {
            GameObject bullet = Instantiate(MobDta.bulletPf, transform.position, transform.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = (player.transform.position - transform.position).normalized;
            bulletRb.velocity = direction * MobDta.bulletSpeed;
            fireRateTimer = MobDta.fireRate;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && MobDta.mobHealth > 0)
        {
            health -= gunController.damage;
            Debug.Log("getHit! Health remain: " + health);

            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            forceToApply += new Vector2(knockbackDirection.x * knockbackForce, knockbackDirection.y * knockbackForce);
        }
    }
}
