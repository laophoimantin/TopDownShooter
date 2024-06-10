using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Transform player;
    [SerializeField] private float smoothing = 0.125f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 targetPosition = player.position;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothing); //Create smooth movements
        smoothedPosition.z = transform.position.z; //Lock z position
        transform.position = smoothedPosition; //Update the camera's position
    }
}
