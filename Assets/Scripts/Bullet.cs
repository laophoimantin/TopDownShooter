using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitEffect;
    private int bulletPierceAmount;
    private float bulletLifeTime;
    private float bulletDamage;

    private bool mobFound;

    //GunController script
    private GunController gunControl;
    void Start()
    {
        gunControl = GameObject.FindGameObjectWithTag("Player").GetComponent<GunController>();

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
