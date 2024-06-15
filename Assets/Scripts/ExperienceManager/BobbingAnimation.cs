using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingAnimation : MonoBehaviour
{
    [SerializeField] public float frequency; // Speed of movement;
    [SerializeField] private float magnitude; // Range of movement;
    [SerializeField] private Vector3 direction; // Direction of movement
    private Vector3 initialPosition;
    private bool isCollected = false;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        if (!isCollected) 
        {
            transform.position = initialPosition + direction * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collector"))
        {
            isCollected = true;
        }
    }
}
