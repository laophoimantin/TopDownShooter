using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private PlayerVisual _visual;
    private WeaponController _weapon;


    public PlayerMovement Movement => _movement;
    public PlayerHealth Health => _health;
    public WeaponController Weapon => _weapon;

    [Header("Test")]
    [SerializeField] private bool _isInvincible;

    public void EquipWeapon(WeaponController newWeapon)
    {
        _weapon = newWeapon;
    }

    public void TakeDamage(float dmg, Vector2 knockbackVector)
    {
        if (_isInvincible) return;

        if (_health.IsInvincible || _health.IsDead)
        {
            return;
        }

        _health.DecreaseHealth();
        _movement.TakeKnockback(knockbackVector);
    }
}