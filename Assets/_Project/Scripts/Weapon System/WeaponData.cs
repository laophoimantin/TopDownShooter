using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Config")]
	public string weaponName;
	public GameObject bulletPrefab;

	[Header("Gun Stats")]
    public float fireRate;

	[Header("Bullet Payload")]
	public float bulletSpeed;
    public float damage;
    public float bulletLifeTime;
    public int pierceCount;
    public float knockbackForce;

    [Header("Apperance")]
    public Sprite Image;
    public Sprite bulletSP;
	public AudioClip fireSound;
}
