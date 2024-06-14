using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{

    private MainPlayerMovement playerMovementScript;
    private AdvancedGunController playerGunControllerScript;
    private PlayerStats playerStats;

    void Start()
    {
        playerMovementScript = FindObjectOfType<MainPlayerMovement>();    
        playerGunControllerScript = FindObjectOfType<AdvancedGunController>();
        playerStats = FindObjectOfType<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UpgradeMoveSpeed();
            UpgradeFireRate();
            UpgradeMaxHealth();
        }
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
        Debug.Log("Gun shoots faster: " + playerGunControllerScript.fireRate);
    }

}
