using UnityEngine;

public class MobVisuals : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _visual;
    private MobData _data;
    private Transform _target;

    public void Init(MobData data, Transform target)
    {
        _data = data;
        _target = target;
        if (_visual != null && _data.Image != null)
            _visual.sprite = _data.Image;
    }

    public void OnUpdate()
    {
        if (_target == null) return;
        _visual.flipX = _target.position.x < transform.position.x;
    }

    public void OnFixedUpdate()
    {
        
    }

}