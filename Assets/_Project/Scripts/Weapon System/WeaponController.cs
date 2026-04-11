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

    private float _currentDamage;
    private float _currentRange;
    private float _currentFireRate;
    private int _currentPierceCount;
    private float _fireRateTimer;
    public static event Action OnFire;

    void Start()
    {
        _currentDamage = _data.damage;
        _currentRange = _data.bulletLifeTime;
        _currentFireRate = _data.fireRate;
        _currentPierceCount = _data.pierceCount;
    }

    public void OnUpdate()
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
            _fireRateTimer = _currentFireRate;
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
                proj.Setup(_currentDamage, _data.bulletSpeed, _currentRange, _currentPierceCount, _data.knockbackForce);
            }
        }

        OnFire?.Invoke();
    }

    public void AddDamage(float dmg)
    {
        _currentDamage += dmg;
    }

    public void AddRange(float range)
    {
        _currentRange += range;
    }
    public void AddPierceCount(int count)
    {
        _currentPierceCount += count;
    }
    public void AddFireRate(float rate)
    {
        _currentFireRate -= rate;
        if (_currentFireRate <= 0)
        {
            _currentFireRate = 0.1f;
        }
    }
}
