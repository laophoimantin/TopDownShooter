using System.Collections.Generic;
using UnityEngine;

namespace User.Manager.Upgrade
{
    using Scriptable.Upgrade;

    public enum UpgradeType
    {
        Range,
        Health,
        Damage,
        Speed,
        Pierce,
        FireRate
    }

    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance;

        [SerializeField] private List<UpgradeData> availableUpgrades = new();

        [Header ("SCRIPTS")]
        private MainPlayerMovement playerMovement;
        private AdvancedGunController gunStats;
        private PlayerStats playerStats;

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        void Start()
        {
            playerMovement = FindObjectOfType<MainPlayerMovement>();
            gunStats = FindObjectOfType<AdvancedGunController>();
            playerStats = FindObjectOfType<PlayerStats>();
        }

        public static UpgradeData GetRandomUpgrade()
        {
            return Instance.availableUpgrades[Random.Range(0, Instance.availableUpgrades.Count)];
        }

        public List<UpgradeData> GetThreeRandomUpgrades()
        {
            List<UpgradeData> tempUpgrades = new List<UpgradeData>(availableUpgrades);

            while (tempUpgrades.Count > 3)
            {
                int randomIndex = Random.Range(0, tempUpgrades.Count);
                tempUpgrades.RemoveAt(randomIndex);
            }

            return tempUpgrades;
        }

        public void GetUpgradeFromType(UpgradeType _type)
        {
            switch (_type)
            {
                case UpgradeType.Health:
                    UpgradeHealth();
                    break;
                case UpgradeType.Damage:
                    UpgradeDamage();
                    break;
                case UpgradeType.Speed:
                    UpgradeSpeed();
                    break;
                case UpgradeType.Range:
                    UpgradeRange();
                    break;
                case UpgradeType.Pierce:
                    UpgradePierce();
                    break;
                case UpgradeType.FireRate:
                    UpgradeFireRate();
                    break;

            }
        }

        public  void UpgradeHealth()
        {
            playerStats.playerMaxHealth += 1;
            playerStats.RestoreHealth();
        }

        public  void UpgradeDamage()
        {
            gunStats.damage += 0.3f;
        }

        public  void UpgradeSpeed()
        {
            gunStats.originalMoveSpeed += 0.2f;
        }

        public  void UpgradeRange()
        {
            gunStats.bulletLifeTime += 0.06f;
        }

        public  void UpgradePierce()
        {
            gunStats.pierceCount += 1;
        }

        public  void UpgradeFireRate()
        {
            gunStats.fireRate -= 0.05f;
        }
    }
}

