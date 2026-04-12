using UnityEngine;

public class CollectibleSP : MonoBehaviour
{
    [Header("Settings")]
    public float PullSpeed = 15f;
    
    [HideInInspector] public Vector3 LogicalPosition; 
    [HideInInspector] public bool IsPulled = false;
    [HideInInspector] public int ListIndex = -1;

    private ICollectible _collectibleEffect;

    public void Init(Vector3 dropPos)
    {
        LogicalPosition = dropPos;
        transform.position = dropPos;
        IsPulled = false;
        
        if (_collectibleEffect == null) 
            _collectibleEffect = GetComponent<ICollectible>();
    }

    public void Collect(GameObject player)
    {
        if (_collectibleEffect != null)
        {
            _collectibleEffect.Collect(player); 
        }
        PoolManager.Instance.Despawn(gameObject);
    }
}