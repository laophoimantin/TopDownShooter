using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedGunController : MonoBehaviour
{
    private MainPlayerMovement playerMovement;

    //Gun handle
    [Header("Gun")]
    private Transform hand;
    [SerializeField] private float gunDistance = 1.5f;
    private bool gunFacingRight = true;
    private Vector3 mousePos;
    private Vector3 mouseDirection;

    [Header("GunSelection")]
    [SerializeField] private int gunType = 1;
    [SerializeField] private GameObject bulletPf;
    [SerializeField] private GunData handGunData;
    [SerializeField] private GunData shotGunData;
    [SerializeField] private Transform[] handGunFirePoint;
    [SerializeField] private Transform[] shotGunFirePoint;
    public GunData gunData;

    //Weapon stats
    [Header("GunStats")]
    private Transform[] activeFirePoints;
    private float bulletSpeed;
    private float fireRate;
    public float damage;
    public float bulletLifeTime;
    public int pierceCount;
    private float fireRateTimer;

    //Reduce player speed while shooting
    [Header("Slow")]
    [SerializeField] private float slowDownDuration = 0.5f;
    private float slowDownTimer;
    private float originalMoveSpeed;

    // Gun Sprire
    [Header("Gun Sprite")]
    [SerializeField] private SpriteRenderer handPos;

    private void Awake()
    {
        GetGunType();
    }

    void Start()
    {
        handPos.sprite = gunData.Image;
        hand = transform.Find("Hand");
        fireRateTimer = fireRate;
        playerMovement = GetComponent<MainPlayerMovement>();
        originalMoveSpeed = playerMovement.moveSpeed;
    }

    void Update()
    {
        GunPositionHandle();

        slowDownTimer -= Time.deltaTime;
        // Update the fire rate cooldown
        if (fireRateTimer > 0)
        {
            fireRateTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (fireRateTimer <= 0)
            {
                Shoot();
                fireRateTimer = fireRate;
                slowDownTimer = slowDownDuration;
            }
        }

        if (slowDownTimer > 0)
        {
            playerMovement.moveSpeed = originalMoveSpeed * 0.7f;
        }
        else
        {
            playerMovement.moveSpeed = originalMoveSpeed;
        }
    }
    private void GetGunType()
    {
        if (gunType == 1)
        {
            gunData = handGunData;
            activeFirePoints = handGunFirePoint; 
        }
        else if (gunType == 2)
        {
            gunData = shotGunData;
            activeFirePoints = shotGunFirePoint; 
        }
        else
        {
            Debug.LogError("Invalid gun type!");
        }
        SetGunType();
    }

    private void SetGunType()
    {
        bulletSpeed = gunData.bulletSpeed;
        fireRate = gunData.fireRate;
        damage = gunData.damage;
        bulletLifeTime = gunData.bulletLifeTime;
        pierceCount = gunData.pierceCount;
    }

    private void Shoot()
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
