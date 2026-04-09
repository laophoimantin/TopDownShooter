using System;
using JetBrains.Annotations;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponVisuals _visuals;

    [Header("Data")]
    [SerializeField] private WeaponData _data;
    [SerializeField] private Transform[] _shootPoint;

    private float _fireRateTimer;
    public static event Action OnFire;

    void Update()
    {
        if (_fireRateTimer > 0)
        {
            _fireRateTimer -= Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            AttemptFire();
        }
    }

    private void AttemptFire()
    {
        if (_fireRateTimer <= 0)
        {
            Shoot();
            _fireRateTimer = _data.fireRate;
        }
    }

    private void Shoot()
    {
        if (_data.fireSound != null)
        {
            SoundManager.Instance.PlaySFX(_data.fireSound);
        }

        foreach (Transform firePoint in _shootPoint)
        {
            GameObject bullet = Instantiate(_data.bulletPrefab, firePoint.position, firePoint.rotation);

            if (bullet.TryGetComponent(out Projectile proj))
            {
                proj.Setup(_data.damage, _data.bulletSpeed, _data.bulletLifeTime, _data.pierceCount, _data.knockbackForce);
            }
        }

        OnFire?.Invoke();
    }
}
