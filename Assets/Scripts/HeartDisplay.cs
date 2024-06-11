using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerStats))]
public class HeartDisplay : MonoBehaviour
{

    private PlayerStats playerStats;
    private int currentHealth;
    private int maxHealth;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;


    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        maxHealth = playerStats.playerMaxHealth;
    }

    void Update()
    {
        currentHealth = playerStats.playerCurrentHealth;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else 
            {
                hearts[i].sprite = emptyHeart;
            }
            if(i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
