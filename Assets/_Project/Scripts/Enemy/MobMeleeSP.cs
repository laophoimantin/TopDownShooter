using UnityEngine;

public class MobMeleeSP : MonoBehaviour
{
    private MobData _data;
    private float _attackTimer;

    public void Init(MobData data)
    {
        _data = data;
        _attackTimer = 0f;
    }

    public void TickCooldown(float dt)
    {
        if (_attackTimer > 0)
        {
            _attackTimer -= dt;
        }
    }

    public bool TryAttack(PlayerController victim, Vector3 myPos)
    {
        if (_attackTimer <= 0)
        {
            Vector2 knockbackDir = ((Vector2)victim.transform.position - (Vector2)myPos).normalized;
            victim.TakeDamage(1, knockbackDir);
            _attackTimer = 1; 
            
            return true;
        }
        
        return false;
    }
}