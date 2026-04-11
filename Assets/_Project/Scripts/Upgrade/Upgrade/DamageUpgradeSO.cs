using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "ScriptableObjects/Damage")]

public class DamageUpgradeSO : UpgradeData 
{
    public override void ApplyUpgrade(PlayerController player)
    {
        player.Weapon.AddDamage(_power);
    }
}
