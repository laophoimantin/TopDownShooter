using System;
using JetBrains.Annotations;
using UnityEditor.Rendering;
using UnityEngine;

public class WeaponController : MonoBehaviour, IUpdater
{
    [Header("Data")]
    [SerializeField] private WeaponData _data;
    [SerializeField] private Transform[] _shootPoint;

    // -----------------------------------------------
    private float _currentDamage;
    private float _currentRange;
    private float _currentFireRate;
    private int _currentPierceCount;
    private float _fireRateTimer;

    // =================================================
    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.OnUnassignUpdater(this);
        }
    }

    public void Init()
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
        if (_data.fireSound != null && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(_data.fireSound);
        }

        for (int i = 0; i < _shootPoint.Length; i++)
        {
            Transform firePoint = _shootPoint[i];
            Vector3 firePos = firePoint.position;
            
            Vector3 fireDirection = firePoint.right;
            GameObject bullet = PoolManager.Instance.Spawn(_data.bulletPrefab, firePos, firePoint.rotation);
            
            if (bullet.TryGetComponent(out ProjectileSP projSP))
            {
                projSP.Setup(_currentDamage, _data.bulletSpeed, _currentRange, _currentPierceCount, _data.knockbackForce, fireDirection, firePos);
            }
        }
    }

    
    // Upgrades
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

    public void ReduceFireCooldown(float rate)
    {
        _currentFireRate -= rate;
        if (_currentFireRate <= 0)
        {
            _currentFireRate = 0.1f;
        }
    }
}