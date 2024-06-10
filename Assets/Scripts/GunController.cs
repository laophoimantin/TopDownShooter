using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunController : MonoBehaviour
{
    //Gun position handle
    [Header("Gun")]
    private Transform gun;
    [SerializeField] private float gunDistance = 1.5f;
    private bool gunFacingRight = true;

    //Shoot
    [Header("Shooting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPf;
    [SerializeField] private float bulletForce = 20f;



    void Start()
    {
        gun = transform.Find("Gun");
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = (mousePos - transform.position).normalized;

        gun.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        gun.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0, 0);

        GunFlipHandle(mousePos);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot(direction);
        }
    }

    private void Shoot(Vector3 direction)
    {
        Debug.Log("Bang!");
        GameObject bullet = Instantiate(bulletPf, firePoint.position, firePoint.rotation);
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        //bulletRb.AddForce(direction * bulletForce, ForceMode2D.Impulse);
        bulletRb.velocity = direction * bulletForce;
    }

    private void GunFlipHandle(Vector3 mousePos)
    {
        if (mousePos.x < gun.position.x && gunFacingRight)
        {
            GunFlip();
        }
        else if (mousePos.x > gun.position.x && !gunFacingRight)
        {
            GunFlip();
        }
    }

    private void GunFlip()
    {
        gunFacingRight = !gunFacingRight;
        gun.localScale = new Vector3(gun.localScale.x, gun.localScale.y * -1, gun.localScale.z);
    }
}
