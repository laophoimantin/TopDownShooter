using System;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    private float _currentHealth;
    private MobData _data;
    private EnemySpawner _spawner;
    [SerializeField] private DropRateManager _dropper;
    
    public void Init(MobData data, EnemySpawner spawner)
    {
        _data = data;
        _spawner = spawner;
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
        if (_spawner != null) 
            _spawner.OnEnemyKilled();
        if (_dropper != null)
        {
            _dropper.DropItem();
        }
        PoolManager.Instance.Despawn(gameObject);
    }
}