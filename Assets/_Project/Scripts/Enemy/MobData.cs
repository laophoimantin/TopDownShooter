using UnityEngine;

[CreateAssetMenu(fileName = "MobData", menuName = "Mob/Mob Data", order = 0)]
public class MobData : ScriptableObject
{
    [Header("BasicMovement")]
    public float mobHealth;
    public float attackRange;
    public float knockbackResistance;
    public float despawnDistance = 40f;
    
    [Header("Movement Settings")]
    public float mobSpeed = 3f;
    public float separationRadius = 0.6f;
    public float separationWeight = 10f;
    
    [Header("Physics")]
    public float mass = 1f;
    
    [Header("Visual")]
    public GameObject blood;
}