using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelector : MonoBehaviour
{
    public static GunSelector instance;
    public GunTypeSelection gunType;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GunTypeSelection GetCurrentGunType()
    {
        return instance.gunType;
    }
    
    public void SelectGun(GunTypeSelection gun)
    {
        gunType = gun;
    }

    public void DestroySingleton()
    {
        instance = null;
        Destroy(gameObject);
    }
}
