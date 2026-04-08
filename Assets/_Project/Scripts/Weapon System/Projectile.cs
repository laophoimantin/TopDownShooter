using UnityEngine;

public class Projectile : MonoBehaviour
{
	private float _damage;
	private int _pierceCount;
	private float _knockbackForce;

	[SerializeField] private Rigidbody2D _rb;
	[SerializeField] private GameObject hitEffect;

	public void Setup(float dmg, float speed, float lifetime, int pierce, float knockback)
	{
		_damage = dmg;
		_pierceCount = pierce;
		_knockbackForce = knockback;

		_rb.velocity = transform.right * speed;

		Destroy(gameObject, lifetime);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.TryGetComponent(out IDamageable victim))
		{

			Vector2 knockbackDir = (collision.transform.position - transform.position).normalized;
			Vector2 finalKnockback = knockbackDir * _knockbackForce;

			victim.TakeDamage(_damage, finalKnockback);

			if (_pierceCount > 0)
			{
				_pierceCount--;
			}
			else
			{
				Explode();
			}
		}
		else if (collision.CompareTag("Wall"))
		{
			Explode();
		}
	}

	private void Explode()
	{
		if (hitEffect != null)
		{
			GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
			Destroy(effect, 1f);
		}
		Destroy(gameObject);
	}
}