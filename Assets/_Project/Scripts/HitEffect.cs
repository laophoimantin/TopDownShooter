using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1f;
    private float _timer;

    // BẮT BUỘC: Reset lại đồng hồ mỗi khi được lôi từ kho ra
    void OnEnable()
    {
        _timer = 0f; 
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _lifeTime)
        {
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}