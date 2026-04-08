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
    }

    void Update()
    {
        maxHealth = playerStats.playerMaxHealth;
        currentHealth = playerStats.playerCurrentHealth;

        // Iterate through each heart image to display current health
        for (int i = 0; i < hearts.Length; i++)
        {
            // Set heart sprite based on current health
            if (i < currentHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else 
            {
                hearts[i].sprite = emptyHeart;
            }

            // Enable/disable heart images based on max health
            if (i < maxHealth)
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
