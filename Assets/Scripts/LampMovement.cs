using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampMovement : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private Vector2 offset;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector2 targetPosition = player.transform.position;
            transform.position = targetPosition + offset;
        }
    }
}
