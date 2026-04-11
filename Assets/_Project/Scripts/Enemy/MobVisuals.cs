using UnityEngine;

public class MobVisuals : MonoBehaviour , IUpdater
{
    [SerializeField] private SpriteRenderer _visual;
    private MobData _data;
    private Transform _target;

    public void Init(MobData data, Transform target)
    {
        _data = data;
        _target = target;
    }

    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance. OnUnassignUpdater(this);
        }
    }
    
    public void OnUpdate()
    {
        if (_target == null) return;
        _visual.flipX = _target.position.x < transform.position.x;
    }
}