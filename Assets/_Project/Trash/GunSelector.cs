using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: delete
public class GunSelector : Singleton<GunSelector>
{
    public GunTypeSelection gunType;

    public static GunTypeSelection GetCurrentGunType()
    {
        return Instance.gunType;
    }
    
    public void SelectGun(GunTypeSelection gun)
    {
        gunType = gun;
    }

    public void DestroySingleton()
    {
        Destroy(gameObject);
    }
}
