using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "Mob/Mob Data", order = 0)]
public class MobData : ScriptableObject
{
    [Header("BasicMovement")]
    public float mobSpeed;
    public float mobHealth;
    public float attackRange;
    public float knockbackResistance;
    public float despawnDistance = 40f;
    public float KnockbackForce;

    [Header("Visual")]
    public GameObject blood;
}