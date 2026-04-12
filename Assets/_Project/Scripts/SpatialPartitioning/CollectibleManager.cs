using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : Singleton<CollectibleManager>, IUpdater
{
    [SerializeField] private PlayerController _player;

    // ------------------------------------------------------------
    public List<CollectibleSP> activeItems = new();
    public List<CollectibleSP> pulledItems = new(); 
    
    [SerializeField] private float _magnetRadius = 3f;
    [SerializeField] private float _pickupRadius = 0.5f; 

    [Header("Global Float Animation")]
    [SerializeField] private float _frequency = 3f;
    [SerializeField] private float _magnitude = 0.1f;

    private List<CollectibleSP> _nearbyItems = new();

    void OnEnable() { UpdateManager.Instance.OnAssignUpdater(this); }
    void OnDisable() { if (UpdateManager.Instance != null) UpdateManager.Instance.OnUnassignUpdater(this); }

    public void RegisterItem(CollectibleSP item, Vector3 pos)
    {
        item.Init(pos);
        item.ListIndex = activeItems.Count;
        activeItems.Add(item);
        
        CollectibleGrid.Instance.Register(item); 
    }

  public void OnUpdate()
    {
        if (_player == null || activeItems.Count == 0) return;
        
        CollectibleGrid.Instance.ClearGrid();
        for (int i = 0; i < activeItems.Count; i++)
        {
            if (!activeItems[i].IsPulled)
            {
                CollectibleGrid.Instance.Register(activeItems[i]);
            }
        }
        
        Vector3 playerPos = _player.transform.position;
        float dt = Time.deltaTime;

        float globalFloatOffset = Mathf.Sin(Time.time * _frequency) * _magnitude;
        Vector3 floatVec = new Vector3(0, globalFloatOffset, 0);

        _nearbyItems.Clear();
        CollectibleGrid.Instance.GetNearbyItems(playerPos, _magnetRadius, ref _nearbyItems);

        float sqrMagnet = _magnetRadius * _magnetRadius;
        for (int i = 0; i < _nearbyItems.Count; i++)
        {
            CollectibleSP item = _nearbyItems[i];
            if (item.IsPulled) continue;

            float sqrDist = (item.LogicalPosition - playerPos).sqrMagnitude;
            if (sqrDist <= sqrMagnet)
            {
                item.IsPulled = true;
                pulledItems.Add(item);
            }
        }

        for (int i = 0; i < activeItems.Count; i++)
        {
            CollectibleSP item = activeItems[i];
            if (!item.IsPulled)
            {
                item.transform.position = item.LogicalPosition + floatVec;
            }
        }

        float sqrPickup = _pickupRadius * _pickupRadius;
        for (int i = pulledItems.Count - 1; i >= 0; i--)
        {
            CollectibleSP item = pulledItems[i];
            
            item.LogicalPosition = Vector3.MoveTowards(item.LogicalPosition, playerPos, item.PullSpeed * dt);
            item.transform.position = item.LogicalPosition; 

            if ((item.LogicalPosition - playerPos).sqrMagnitude <= sqrPickup)
            {
                item.Collect(_player.gameObject);
                
                pulledItems.RemoveAt(i);
                
                int itemIndex = item.ListIndex;
                int lastIndex = activeItems.Count - 1;
                
                CollectibleSP lastItem = activeItems[lastIndex]; 
                activeItems[itemIndex] = lastItem;             
                lastItem.ListIndex = itemIndex;             
                activeItems.RemoveAt(lastIndex);               
            }
        }
    }
}