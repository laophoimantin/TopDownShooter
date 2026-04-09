using System;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : MonoBehaviour
{
    [Header("References")]
    private Animator anim;
    [SerializeField] private GameObject dummy;

    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 3;
    private int _currentHealth;
    public int CurrentHealth;

    [Header("Invincibility Settings")]
    [HideInInspector] public float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 3f;

    public event Action<float> OnTakeDamage;
    public event Action<int> OnHealthChange;
    

    void Start()
    {
        _currentHealth = _maxHealth;
        anim = GetComponent<Animator>();
        GameManager.Instance.AssignLevelReached(level);

        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if(_currentHealth <= 0)
        {
            Die();
        }

        if(invincibilityTimer >= 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        // Max health cap
        if (_maxHealth > 8)
        {
            _maxHealth = 8;
        }
        //GameManager.Instance.CurrentLVDisplay(level);
    }

    public void GetHit()
    {
        _currentHealth -= 1;
        invincibilityTimer = invincibilityDuration;
        OnTakeDamage?.Invoke(invincibilityDuration);
        SoundManager.Instance.PlaySFX(SoundManager.Instance.hurtSoundClip, SoundManager.Instance.otherSoundSource);
    }

    private void Die()
    {
        Instantiate(dummy, transform.position, transform.rotation);
        Destroy(gameObject);
        GameManager.Instance.Delay(false, 2);
    }

    public void RestoreHealth()
    {
        if (_currentHealth < _maxHealth)
        {
            _currentHealth += 1;
            SoundManager.Instance.PlaySFX(SoundManager.Instance.healSoundClip, SoundManager.Instance.otherSoundSource);
        }
    }

    // Experience and level handle
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    public List<LevelRange> levelRanges;

    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelUpChecker();
    }


    void LevelUpChecker()
    {
        if (experience >= experienceCap)
        {
            level++;
            experience -= experienceCap;

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if (level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            SoundManager.Instance.PlaySFX(SoundManager.Instance.levelUpSoundClip, SoundManager.Instance.otherSoundSource);
            GameManager.Instance.AssignLevelReached(level);
            GameManager.Instance.StartLevelUp();
        }
    }

}
