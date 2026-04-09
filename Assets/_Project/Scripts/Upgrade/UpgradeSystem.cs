using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystem : MonoBehaviour
{

    private PlayerMovement playerMovementScript;
    private AdvancedGunController playerGunControllerScript;
    private PlayerHealth playerStats;

    [SerializeField] private Button[] upgradeButtons;


    void Start()
    {
        playerMovementScript = FindObjectOfType<PlayerMovement>();    
        playerGunControllerScript = FindObjectOfType<AdvancedGunController>();
        playerStats = FindObjectOfType<PlayerHealth>();
    }


    void UpgradeMoveSpeed()
    {
        playerGunControllerScript.originalMoveSpeed += 1;
        //Debug.Log("Player is faster: " + playerMovementScript._moveSpeed);
    }

    void UpgradeMaxHealth()
    {
        //playerStats._maxHealth += 1;
        //Debug.Log("Player is stronger: " + playerStats._maxHealth);
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
