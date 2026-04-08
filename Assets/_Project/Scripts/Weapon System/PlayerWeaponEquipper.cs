using UnityEngine;

public class PlayerWeaponEquipper : MonoBehaviour
{
	[SerializeField] private PlayerLoadout _currentLoadout;
	[SerializeField] private Transform _weaponHoldPoint;

	void Start()
	{
		if (_currentLoadout.GetSelectedWeapon() == null || _currentLoadout.GetSelectedWeapon().WeaponPrefab == null)
		{
			return;
		}

		Instantiate(_currentLoadout.GetSelectedWeapon().WeaponPrefab, _weaponHoldPoint.position, _weaponHoldPoint.rotation, _weaponHoldPoint);
	}
}