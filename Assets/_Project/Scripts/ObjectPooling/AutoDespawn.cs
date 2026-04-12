using UnityEngine;

public class AutoDespawn : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 2f; 
    private float _timer;

    private void OnEnable()
    {
        _timer = _lifeTime; 
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}