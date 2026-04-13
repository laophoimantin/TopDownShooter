using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropper : MonoBehaviour
{
    [SerializeField] private MobHealth _health;
    [SerializeField] private float _dropRadius = 0.3f;

    [System.Serializable]
    public class Drops
    {
        public string Name;
        public GameObject ItemPrefab;
        public float DropRate;
    }

    [SerializeField] private List<Drops> _drops;

    void OnEnable()
    {
        _health.OnDeath += DropItem;
    }

    void OnDisable()
    {
        _health.OnDeath -= DropItem;
    }

    public void DropItem()
    {
        foreach (Drops rate in _drops)
        {
            float randomNumber = Random.Range(0f, 100f);

            if (randomNumber <= rate.DropRate)
            {
                Vector2 randomOffset = Random.insideUnitCircle * _dropRadius;
                Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
                GameObject itemObj = PoolManager.Instance.Spawn(rate.ItemPrefab, spawnPosition, Quaternion.identity);

                if (itemObj.TryGetComponent(out CollectibleSP gemSP))
                {
                    CollectibleManager.Instance.RegisterItem(gemSP, spawnPosition);
                }
            }
        }
    }
}