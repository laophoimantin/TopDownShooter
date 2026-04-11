using UnityEngine;

public class PlayerVisual : MonoBehaviour, IUpdater
{
    private static readonly int IsInvincible = Animator.StringToHash("IsInvincible");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Die = Animator.StringToHash("Die");
    
    [SerializeField] private Animator _anim;
    [SerializeField] private Rigidbody2D _rb;

    private void OnEnable()
    {
        PlayerHealth.OnInvincibilityChanged += SetGetHitState;
        PlayerHealth.OnDeathStarted += PlayDeathAnim;
        
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    private void OnDisable()
    {
        PlayerHealth.OnInvincibilityChanged -= SetGetHitState;
        PlayerHealth.OnDeathStarted -= PlayDeathAnim;
        
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance. OnUnassignUpdater(this);
        }
    }

    public void OnUpdate()
    {
        ChangeAnimState();
    }

    private void PlayDeathAnim()
    {
        _anim.SetTrigger(Die);
    }
    
    private void ChangeAnimState()
    {
        bool isMoving = _rb.velocity.sqrMagnitude > 0.01f;
        _anim.SetBool(IsWalking, isMoving);
    }

    private void SetGetHitState(bool state)
    {
        _anim.SetBool(IsInvincible, state);
    }
}