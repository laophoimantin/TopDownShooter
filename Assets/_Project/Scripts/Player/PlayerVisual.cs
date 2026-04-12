using UnityEngine;

public class PlayerVisual : MonoBehaviour, IUpdater
{
    private static readonly int IsInvincible = Animator.StringToHash("IsInvincible");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Die = Animator.StringToHash("Die");

    [Header("References")]
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    private void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.OnUnassignUpdater(this);
        }
    }

    public void OnUpdate()
    {
        ChangeAnimState();
    }

    public void PlayDeathAnim()
    {
        _anim.SetTrigger(Die);
    }

    private void ChangeAnimState()
    {
        bool isMoving = _rb.velocity.sqrMagnitude > 0.01f;
        _anim.SetBool(IsWalking, isMoving);
    }

    public void SetGetHitState(bool state)
    {
        _anim.SetBool(IsInvincible, state);
    }
}