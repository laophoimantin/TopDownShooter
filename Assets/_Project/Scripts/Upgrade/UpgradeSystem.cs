using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{

    private MainPlayerMovement playerMovementScript;
    private AdvancedGunController playerGunControllerScript;
    private PlayerStats playerStats;

    [SerializeField] private Button[] upgradeButtons;


    void Start()
    {
        playerMovementScript = FindObjectOfType<MainPlayerMovement>();    
        playerGunControllerScript = FindObjectOfType<AdvancedGunController>();
        playerStats = FindObjectOfType<PlayerStats>();
    }


    void UpgradeMoveSpeed()
    {
        playerGunControllerScript.originalMoveSpeed += 1;
        Debug.Log("Player is faster: " + playerMovementScript.moveSpeed);
    }

    void UpgradeMaxHealth()
    {
        playerStats.playerMaxHealth += 1;
        Debug.Log("Player is stronger: " + playerStats.playerMaxHealth);
    }

    void UpgradeFireRate()
    {
        playerGunControllerScript.fireRate -= 0.01f;
        Debug.Log("Shoot faster: " + playerGunControllerScript.fireRate);
    }

    private static void UpgradeDamage(ref AdvancedGunController playerGunControllerScript)
    {
        playerGunControllerScript.damage += 1f;
        Debug.Log("More pain: " + playerGunControllerScript.fireRate);
    }   
    void UpgradeRange()
    {
        playerGunControllerScript.bulletLifeTime += 1f;
        Debug.Log("More pain: " + playerGunControllerScript.fireRate);
    }

    void UpgradePiercing()
    {
        playerGunControllerScript.pierceCount += 1;
    }

}
