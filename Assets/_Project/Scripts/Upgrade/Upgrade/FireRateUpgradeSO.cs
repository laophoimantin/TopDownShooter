using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/FireRate")]
public class FireRateUpgradeSO : UpgradeData
{
    public override void ApplyUpgrade(PlayerController player)
    {
        player.Weapon.ReduceFireCooldown(_power);
    }
}