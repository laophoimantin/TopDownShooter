using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private PlayerVisual _visual;



    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
        _health.GetHit();
    }
}