using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private GameObject dummy;

    public int playerMaxHealth = 3;
    [HideInInspector] public int playerCurrentHealth;
    [HideInInspector] public float invincibilityTimer;
    [SerializeField] private float invincibilityDuration = 3f;

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        anim = GetComponent<Animator>();

        experienceCap = levelRanges[0].experienceCapIncrease;
    }

    void Update()
    {
        if(playerCurrentHealth <= 0)
        {
            Die();
        }

        anim.SetFloat("GetHit", invincibilityTimer);

        if(invincibilityTimer >= 0)
        {
            invincibilityTimer -= Time.deltaTime;
        }

        // Max health cap
        if (playerMaxHealth > 8)
        {
            playerMaxHealth = 8;
        }
    }

    public void GetHit()
    {
        playerCurrentHealth -= 1;
        Debug.Log("Get Hit! Health remain: " + playerCurrentHealth);
        invincibilityTimer = invincibilityDuration;
    }
    private void Die()
    {
        Instantiate(dummy, transform.position, transform.rotation);
        Destroy(gameObject);
        if (!GameManager.instance.isGameOver)
        {
            GameManager.instance.AssignLevelReached(level);
            //StartCoroutine(GameManager.instance.GameOver());
            GameManager.instance.GameOver();
        }
    }

    public void RestoreHealth()
    {
        if (playerCurrentHealth < playerMaxHealth)
        {
            playerCurrentHealth += 1;
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

            GameManager.instance.StartLevelUp();
        }
    }

}
