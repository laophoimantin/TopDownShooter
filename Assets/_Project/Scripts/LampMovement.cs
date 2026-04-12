using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampMovement : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Vector2 _offset;

    void LateUpdate()
    {
        if (_pivot != null)
        {
            Vector2 targetPosition = _pivot.transform.position;
            transform.position = targetPosition + _offset;
        }
    }
}
