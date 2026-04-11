using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleBehavior : MonoBehaviour, IUpdater
{
    [Header("Magnet Settings")]
    [SerializeField] private float _pullSpeed = 15f;
    private Transform _magnetTarget;
    
    [SerializeField] public float _frequency = 3; // Speed of movement;
    [SerializeField] private float _magnitude = 0.1f; // Range of movement;
    [SerializeField] private Vector3 _direction = Vector2.up; // Direction of movement
    private Vector3 _initialPosition;
    private bool _isBeingPulled = false;

    private ICollectible _collectibleEffect;
    
    private void Start()
    {
        _initialPosition = transform.position;
        _collectibleEffect = GetComponent<ICollectible>(); 
    }

    void OnEnable()
    {
        UpdateManager.Instance.OnAssignUpdater(this);
    }

    void OnDisable()
    {
        if (UpdateManager.Instance != null)
            UpdateManager.Instance.OnUnassignUpdater(this);
    }

    public void OnUpdate()
    {
        if (_isBeingPulled && _magnetTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _magnetTarget.position, _pullSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = _initialPosition + _direction * Mathf.Sin(Time.time * _frequency) * _magnitude;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collector") && !_isBeingPulled)
        {
            _magnetTarget = collision.transform;
            _isBeingPulled = true;
        }
        else if (collision.CompareTag("Player"))
        {
            if (_collectibleEffect != null)
            {
                _collectibleEffect.Collect(collision.gameObject); 
            }
            PoolManager.Instance.Despawn(gameObject);
        }
    }
}