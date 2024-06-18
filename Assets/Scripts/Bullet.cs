using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject hitEffect;

    private int bulletPierceAmount;
    private float bulletLifeTime;
    private bool mobFound;

    [Header("References")]
    private AdvancedGunController gunControl;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gunControl = GameObject.FindGameObjectWithTag("Player").GetComponent<AdvancedGunController>();
        spriteRenderer.sprite = gunControl.gunData.bulletSP;

        bulletLifeTime = gunControl.bulletLifeTime;
        bulletPierceAmount = gunControl.pierceCount;
        if (!mobFound)
        {
            Destroy(gameObject, bulletLifeTime);
        }
    }

    private void Update()
    {
        Flip();
    }

    private void Flip()
    {
        if (transform.position.x < GameObject.FindGameObjectWithTag("Player").transform.position.x)
        {
            spriteRenderer.flipY = true;
        }
        else if (transform.position.x > GameObject.FindGameObjectWithTag("Player").transform.position.x)
        {
            spriteRenderer.flipY = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mob"))
        {
            if (bulletPierceAmount > 0)
            {
                bulletPierceAmount -= 1;
            }
            else if (bulletPierceAmount <= 0)
            {
                mobFound = true;
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(effect, 1);
            }
            collision.GetComponent<AdvancedMobController>().GetHit();
        }
        else if (collision.CompareTag("Wall"))
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(effect, 1);
        }
    }

}
