using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "Gun/Gun Data", order = 1)]
public class GunData : ScriptableObject
{
    [Header("Gun's Values")]
    public float bulletSpeed;
    public float fireRate;
    public float damage;
    public float bulletLifeTime;
    public int pierceCount;
    public Sprite Image;
    public Sprite bulletSP;
    public float knockbackForce;
}
