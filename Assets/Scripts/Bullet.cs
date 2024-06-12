using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    private int bulletPierceAmount;
    private float bulletLifeTime;

    private bool mobFound;

    //GunController script
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
        Physics2D.IgnoreLayerCollision(6, 6);
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
                mobFound = false;
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                Destroy(effect, 1);
            }

        }
    }

}
