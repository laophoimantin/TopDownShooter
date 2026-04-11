using UnityEngine;

public class MobMovementSP : MonoBehaviour
{
    private MobData _data;
    
    [HideInInspector] public Vector2 ForceToApply; 
    [HideInInspector] public Vector2 SeparationForce; 
    [HideInInspector] public Vector3 CurrentPos;

    public void Init(MobData data)
    {
        _data = data;
        ForceToApply = Vector2.zero;
        SeparationForce = Vector2.zero;
    }

    public void TakeKnockback(Vector2 knockback)
    {
        ForceToApply += knockback * _data.knockbackResistance;
    }
}