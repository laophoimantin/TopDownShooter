using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunController : MonoBehaviour
{
    //Gun handle
    [Header("Gun")]
    private Transform hand;
    [SerializeField] private float gunDistance = 1.5f;
    private bool gunFacingRight = true;
    private Vector3 mousePos;
    private Vector3 mouseDirection;


    [SerializeField] private GameObject bulletPf;
    //GunSelection
    [SerializeField] private int gunType = 1;

    //HandGun value
    [Header("Handgun")]
    [SerializeField] private Transform[] handGunFirePoints;
    [SerializeField] private float handGunBulletSpeed = 20f;
    [SerializeField] private float handGunFireRate = 0.5f;
    [SerializeField] private float handGunDamamge = 2;
    [SerializeField] private float handGunBulletLifeTime = 2f;
    [SerializeField] private int handGunPierceCount = 0;

    //ShotGun value
    [Header("Shotgun")]
    [SerializeField] private Transform[] shotGunFirePoints;
    [SerializeField] private float shotGunBulletSpeed = 15f;
    [SerializeField] private float shotGunFireRate = 0.8f;
    [SerializeField] private float shotGunDamamge = 6;
    [SerializeField] private float shotGunBulletLifeTime = 1f;
    [SerializeField] private int shotGunPierceCount = 1;

    //Weapon stats
    [Header("GunStats")]
    private Transform[] activeFirePoints;
    private float bulletSpeed;
    private float fireRate;
    public float damage;
    public float bulletLifeTime;
    public int pierceCount;

    private float fireRateCooldown;

    //Reduce player speed while shooting
    private MainPlayerMovement playerMovement;
    private float originalMoveSpeed;

    private void Awake()
    {
        if (gunType == 1)
        {
            activeFirePoints = handGunFirePoints;
            bulletSpeed = handGunBulletSpeed;
            fireRate = handGunFireRate;
            damage = handGunDamamge;
            bulletLifeTime = handGunBulletLifeTime;
            pierceCount = handGunPierceCount;
        }
        else if (gunType == 2)
        {
            activeFirePoints = shotGunFirePoints;
            bulletSpeed = shotGunBulletSpeed;
            fireRate = shotGunFireRate;
            damage = shotGunDamamge;
            bulletLifeTime = shotGunBulletLifeTime;
            pierceCount = shotGunPierceCount;
        }
        else
        {
            Debug.LogError("Invalid gun type!");
        }
    }

    void Start()
    {
        hand = transform.Find("Hand");
        fireRateCooldown = fireRate;

        playerMovement = GetComponent<MainPlayerMovement>();
        originalMoveSpeed = playerMovement.moveSpeed;
    }

    void Update()
    {
        // Update the fire rate cooldown
        if (fireRateCooldown > 0)
        {
            fireRateCooldown -= Time.deltaTime;
        }

        GunPositionHandle();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerMovement.moveSpeed = originalMoveSpeed * 0.7f;
            if (fireRateCooldown <= 0)
            {
                HandGunShoot();
                fireRateCooldown = fireRate;
            }
        }
        else
        {
            playerMovement.moveSpeed = originalMoveSpeed;
        }
    }


    private void HandGunShoot()
    {
        foreach (Transform firePoint in activeFirePoints)
        {
            GameObject bullet = Instantiate(bulletPf, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = mouseDirection * bulletSpeed;
        }
    }

    private void GunPositionHandle()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        mouseDirection = (mousePos - transform.position).normalized;

        hand.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg));

        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        hand.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0, 0);

        GunFlipHandle();
    }
  
    private void GunFlipHandle()
    {
        if (mousePos.x < transform.position.x && gunFacingRight)
        {
            FlipGun();
        }
        else if (mousePos.x > transform.position.x && !gunFacingRight)
        {
            FlipGun();
        }
    }

    private void FlipGun()
    {
        gunFacingRight = !gunFacingRight;
        hand.localScale = new Vector3(hand.localScale.x, hand.localScale.y * -1, hand.localScale.z);
    }
}
