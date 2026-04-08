using UnityEngine;

[CreateAssetMenu(fileName = "PlayerLoadout", menuName = "Weapons/Player Loadout")]
public class PlayerLoadout : ScriptableObject
{
	private WeaponIdentity _selectedWeapon;

	public WeaponIdentity GetSelectedWeapon() => _selectedWeapon;
	public void SetSelectedWeapon(WeaponIdentity selectedWeapon)
	{
		_selectedWeapon = selectedWeapon;
	}
}