using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private Transform player;

    [Header("Camera Settings")]
    [SerializeField] private Camera cam;
    [SerializeField] private float threshold;

    void LateUpdate()
    {
        if (cam != null && player != null)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3 targetPos = (player.position + mousePos) / 2f;
            targetPos.z = 0;

            targetPos.x = Mathf.Clamp(targetPos.x, player.position.x - threshold, player.position.x + threshold);
            targetPos.y = Mathf.Clamp(targetPos.y, player.position.y - threshold, player.position.y + threshold);

            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * 5f);
        }
    }
}