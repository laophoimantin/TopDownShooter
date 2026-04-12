using System;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    private float _currentHealth;
    private MobData _data;
    [SerializeField] private DropRateManager _dropper;
    
    public void Init(MobData data)
    {
        _data = data;
        _currentHealth = _data.mobHealth;
    }

    public void DecreaseHealth(float amount)
    {
        if (_currentHealth <= 0) return;

        _currentHealth -= amount;
        
  
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (EnemySpawner.Instance != null)
            EnemySpawner.Instance.OnEnemyKilled();
        if (_dropper != null)
        {
            _dropper.DropItem();
        }
        PoolManager.Instance.Despawn(gameObject);
    }
}