using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBarManager : MonoBehaviour
{
    [SerializeField] private Image experienceFill;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        float currentXP = playerStats.experience;
        float targetXP = playerStats.experienceCap;
        experienceFill.fillAmount = currentXP / targetXP;
    }
}
