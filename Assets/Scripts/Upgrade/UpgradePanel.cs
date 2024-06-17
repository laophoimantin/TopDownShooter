using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace User.UI
{
    using Manager.General;
    using Manager.Upgrade;
    using User.Scriptable.Upgrade;

    public class UpgradePanel : MonoBehaviour
    {
        [Header("REFERENCES")]
        [SerializeField] private List<UpgradeOption> upgradeOptions;



        private void OnEnable()
        {
            GameManager.OnLevelUp += DisplayRandomUpgrades;
        }

        private void OnDisable()
        {
            GameManager.OnLevelUp -= DisplayRandomUpgrades;
        }

        private void DisplayRandomUpgrades()
        {
            List<UpgradeData> randomUpgrades = UpgradeManager.Instance.GetThreeRandomUpgrades();

            for (int i = 0; i < upgradeOptions.Count && i < randomUpgrades.Count; i++)
            {
                upgradeOptions[i].UpdateDisplay(randomUpgrades[i]);
            }
        }
    }
}

