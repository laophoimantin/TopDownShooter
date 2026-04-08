using UnityEngine;
using UnityEngine.UI;

public class GunSelectionPanel : MonoBehaviour
{
	[Header("Button")]
	[SerializeField] private Button _hangunButton;
	[SerializeField] private Button _shotgunButton;

	[Header("Loadout")]
	[SerializeField] private PlayerLoadout _loadout;

	[Header("Weapon Data")]
	[SerializeField] private WeaponIdentity _handgun;
	[SerializeField] private WeaponIdentity _shotgun;


	void Awake()
	{
		_hangunButton.onClick.AddListener(() => AssignSelectedGun(_handgun));
		_shotgunButton.onClick.AddListener(() => AssignSelectedGun(_shotgun));
	}

	private void AssignSelectedGun(WeaponIdentity data)
	{
		_loadout.SetSelectedWeapon(data);
	}
}