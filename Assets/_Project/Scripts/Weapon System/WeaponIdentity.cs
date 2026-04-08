using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponIdentity", menuName = "Weapons/Weapon Identity")]
public class WeaponIdentity : ScriptableObject
{
	public string WeaponName;
	public GameObject WeaponPrefab;
}