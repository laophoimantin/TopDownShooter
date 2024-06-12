using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "Mob/Mob Data", order = 0)]
public class MobData : ScriptableObject
{
    [Header("BasicMovement")]
    public float mobSpeed;
    public float mobHealth;
    public bool ranger;

    [Header("ProjectileHandle")]
    public GameObject bulletPf;
    public float bulletSpeed;
    public float fireRate;

    [Header("Visual")]
    public Sprite Image;
}
