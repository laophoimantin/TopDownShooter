using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropper : MonoBehaviour
{
    [SerializeField] private MobHealth _health;
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
                GameObject itemObj = PoolManager.Instance.Spawn(rate.ItemPrefab, transform.position, Quaternion.identity);
            
                if (itemObj.TryGetComponent(out CollectibleSP gemSP))
                {
                    CollectibleManager.Instance.RegisterItem(gemSP, transform.position); 
                }
            }
        }
    }
}