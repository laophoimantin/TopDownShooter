using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/Range")]
public class RangeUpgradeSO : UpgradeData
{
    public override void ApplyUpgrade(PlayerController player)
    {
        player.Weapon.AddRange(_power);
    }
}
