using UnityEngine;

public class MobMovementSP : MonoBehaviour
{
    private MobData _data;
    
    public Vector2 _forceToApply; 

    public Vector2 _separationForce; 

    public void Init(MobData data)
    {
        _data = data;
        _forceToApply = Vector2.zero;
        _separationForce = Vector2.zero;
    }

    public void TakeKnockback(Vector2 knockback)
    {
        _forceToApply += knockback * _data.knockbackResistance;
    }
}