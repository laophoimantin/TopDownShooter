using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private PlayerHealth _health;

    private void OnEnable()
    {
        _health.OnTakeDamage += SetGetHitState;
    }

    private void OnDisable()
    {
        
    }
    private void Flip()
    {
        if (_rb.velocity.x < 0)
        {
            _sprite.flipX = true;
        }
        else
        {
            _sprite.flipX = false;
        }
    }

    private void ChangeAnimState()
    {
        if (_rb.velocity != Vector2.zero)
        {
            _anim.SetBool("IsWalking", true);
        }
        else
        {
            _anim.SetBool("IsWalking", false);
        }
    }

    private void SetGetHitState(float duration)
    {
        _anim.SetFloat("GetHit", duration);
    }
}