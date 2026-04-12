using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IUpdater
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;

    // --------------------------------------------------------------------
    [Header("Invincibility Settings")]
    [SerializeField] private float _invincibleDuration = 3f;
    private float _invincibleTimer;
    private bool _isInvincible;

    public bool IsInvincible => _isInvincible;

    // --------------------------------------------------------------------
    [Header("Death Settings")]
    [SerializeField] private float _deathDelay = 2f;
    private bool _isDead = false;
    public bool IsDead => _isDead;

    // --------------------------------------------------------------------
    [Header("Audio")]
    [SerializeField] private AudioClip _hurtSoundClip;
    [SerializeField] private AudioClip _healSoundClip;

    // --------------------------------------------------------------------
    public event Action<bool> OnInvincibilityChanged;
    public event Action OnDeathStarted;

    // ===========================================================================================

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

        // Global: notify external systems (UI, audio, game state)
        this.SendEvent(new OnPlayerDeathStarted());
        
        // Local: notify components on this GameObject
        OnDeathStarted?.Invoke();
        yield return new WaitForSeconds(_deathDelay);

        this.SendEvent(new OnPlayerDeathFinished());
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
        this.SendEvent(new OnPlayerHealthChange
        {
            CurrentHealth = _currentHealth,
            MaxHealth = _maxHealth
        });
    }
}