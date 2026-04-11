using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 1f;
    private float _timer;

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