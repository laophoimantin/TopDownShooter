using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{
    private Dictionary<GameObject, Transform> _poolParents = new();
    
    private readonly Dictionary<GameObject, ObjectPool<GameObject>> _pools = new();
    
    private readonly Dictionary<GameObject, GameObject> _instanceToPrefabMap = new();

    public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!_pools.ContainsKey(prefab))
        {
            GameObject poolFolder = new GameObject($"[Pool] {prefab.name}");
            poolFolder.transform.SetParent(transform);
            _poolParents[prefab] = poolFolder.transform;
            
            _pools[prefab] = new ObjectPool<GameObject>(
                createFunc: () => CreateInstance(prefab),
                actionOnGet: (obj) => 
                { 
                    obj.SetActive(true); 
                },
                actionOnRelease: (obj) => 
                {
                    obj.SetActive(false);
                    obj.transform.SetParent(_poolParents[prefab]); 
                },
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: true,
                defaultCapacity: 20,  
                maxSize: 1000         
            );
        }

        GameObject spawnedObj = _pools[prefab].Get();
        spawnedObj.transform.SetPositionAndRotation(position, rotation);
        return spawnedObj;
    }

    private GameObject CreateInstance(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, _poolParents[prefab]);
        _instanceToPrefabMap[obj] = prefab;
        return obj;
    }

    public void Despawn(GameObject instance)
    {
        if (_instanceToPrefabMap.TryGetValue(instance, out GameObject prefab))
        {
            _pools[prefab].Release(instance);
        }
        else
        {
            Destroy(instance);
        }
    }
}