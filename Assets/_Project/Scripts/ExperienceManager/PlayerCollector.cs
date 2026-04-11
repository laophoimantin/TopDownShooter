using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    // private CircleCollider2D playerCollector;
    // [SerializeField] private float pullSpeed = 300f;
    // [SerializeField] private float pullRadius = 5f;
    // [SerializeField] private LayerMask collectibleLayer;
    //
    // private void Update()
    // {
    //     Vector2 pullCenter = transform.position;
    //     Collider2D[] colliders = Physics2D.OverlapCircleAll(pullCenter, pullRadius, collectibleLayer);
    //
    //     foreach (var collider in colliders)
    //     {
    //         CollectibleBehavior bobbingScript = collider.GetComponent<CollectibleBehavior>();
    //         bobbingScript.isCollected = true; // Disable bobbing to avoid disturbing the pull
    //         if (collider.gameObject.TryGetComponent(out ICollectible collectible))
    //         {
    //             collider.transform.position = Vector2.MoveTowards(collider.transform.position, transform.position, pullSpeed * Time.deltaTime);
    //         }
    //     }
    // }
    //
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawWireSphere(transform.position, pullRadius);
    // }
}
