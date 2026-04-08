using UnityEngine;

public interface IDamageable
{
	public void TakeDamage(float dmg, Vector2 knockbackVector);
}