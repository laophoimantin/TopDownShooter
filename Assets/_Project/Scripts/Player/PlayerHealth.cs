using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IUpdater
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    [Header("Invincibility Settings")]
    [SerializeField] private float _invincibleDuration = 3f;
    private float _invincibleTimer;

    private bool _isInvincible;
    public bool IsInvincible => _isInvincible;

    public static event Action<bool> OnInvincibilityChanged;
    public static event Action<int, int> OnHealthChanged;


    [Header("Death Settings")]
    [SerializeField] private float _deathDelay = 2f;
    private bool _isDead = false;
    public bool IsDead => _isDead;

    public static event Action OnDeathStarted;
    public static event Action OnDeathFinished;

    [Header("Audio")]
    [SerializeField] private AudioClip _hurtSoundClip;
    [SerializeField] private AudioClip _healSoundClip;

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

    
    void Start()
    {
        _currentHealth = _maxHealth;
        UpdateHealthUI();
    }

    public void OnUpdate()
    {
        if (_isInvincible)
        {
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer <= 0)
            {
                _isInvincible = false;
                OnInvincibilityChanged?.Invoke(false);
            }
        }
    }

    public void DecreaseHealth()
    {
        if (_isDead) return;

        ChangeHealth(-1);

        if (_currentHealth <= 0)
        {
            StartCoroutine(DieRoutine());
        }
        else
        {
            _isInvincible = true;
            _invincibleTimer = _invincibleDuration;
            OnInvincibilityChanged?.Invoke(true);

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySfx(_hurtSoundClip);
        }
    }

    public void RestoreHealth()
    {
        if (_currentHealth < _maxHealth)
        {
            ChangeHealth(1);
            
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlaySfx(_healSoundClip);
        }
    }

    private IEnumerator DieRoutine()
    {
        _isDead = true;

        OnDeathStarted?.Invoke();

        yield return new WaitForSeconds(_deathDelay);

        OnDeathFinished?.Invoke();
    }


    private void ChangeHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        UpdateHealthUI();
    }

    public void IncreaseMaxHealth(int amount)
    {
        _maxHealth += amount;
        if (_maxHealth > 8) _maxHealth = 8;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
}