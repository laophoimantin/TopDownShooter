using System;
using UnityEngine;

public class MobHealth : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyDropper _dropper;
    
    private float _currentHealth;
    private MobData _data;
    public event Action OnDeath;

    // =================================================================================================================
    public void Init(MobData data)
    {
        _data = data;
        _currentHealth = _data.mobHealth;
    }

    // ============================================================
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
        this.SendEvent(new OnEnemyKilledEvent());
        
        OnDeath?.Invoke();
        
        PoolManager.Instance.Despawn(gameObject);
    }
}