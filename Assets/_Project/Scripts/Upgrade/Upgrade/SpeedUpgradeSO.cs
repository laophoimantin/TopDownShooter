using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/Speed")]
public class SpeedUpgradeSO : UpgradeData
{
    public override void ApplyUpgrade(PlayerController player)
    {
        player.Movement.AddSpeed(_power);
    }
}
