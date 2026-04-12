using UnityEngine;

public class PlayerWeaponEquipper : MonoBehaviour
{
    [SerializeField] private PlayerLoadout _currentLoadout;
    [SerializeField] private Transform _weaponHoldPoint;
    [SerializeField] private PlayerController _playerController;

    [Space(10)]
    public WeaponIdentity testIdentity;

    void Start()
    {
        GameObject newWeapon = null;
        if (_currentLoadout.GetSelectedWeapon() == null || _currentLoadout.GetSelectedWeapon().WeaponPrefab == null)
        {
#if UNITY_EDITOR
            if (testIdentity != null)
            {
                newWeapon = Instantiate(testIdentity.WeaponPrefab, _weaponHoldPoint.position, _weaponHoldPoint.rotation, _weaponHoldPoint);
                EquipWeapon(newWeapon);
            }
#endif
            return;
        }

        newWeapon = Instantiate(_currentLoadout.GetSelectedWeapon().WeaponPrefab, _weaponHoldPoint.position, _weaponHoldPoint.rotation, _weaponHoldPoint);
        EquipWeapon(newWeapon);
    }

    private void EquipWeapon(GameObject newWeapon)
    {
        WeaponController weaponCon = newWeapon.GetComponent<WeaponController>();
        weaponCon.Init();
        _playerController.EquipWeapon(weaponCon);
    }
}