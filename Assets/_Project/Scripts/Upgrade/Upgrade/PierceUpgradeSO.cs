using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/Pierce")]
public class PierceUpgradeSO : UpgradeData
{
    public override void ApplyUpgrade(PlayerController player)
    {
        player.Weapon.AddPierceCount((int)_power);
    }
}
