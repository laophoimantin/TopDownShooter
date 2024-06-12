using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Gun/Gun Data", order = 1)]
public class GunData : ScriptableObject
{

    //HandGun value
    [Header("Handgun")]
    public float bulletSpeed;
    public float fireRate;
    public float damage;
    public float bulletLifeTime;
    public int pierceCount;
    public Sprite Image;
    public Sprite bulletSP;
}
