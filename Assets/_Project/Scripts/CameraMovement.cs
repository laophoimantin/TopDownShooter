using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	[SerializeField] private Transform _target;

    [Header("Camera Settings")]
    [SerializeField] private Camera _cam;
    [SerializeField] private float _threshold = 0.4f;

    void LateUpdate()
    {
        if (_cam != null && _target != null)
        {
            Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            Vector3 targetPos = (_target.position + mousePos) / 2f;
            targetPos.z = 0;

            targetPos.x = Mathf.Clamp(targetPos.x, _target.position.x - _threshold, _target.position.x + _threshold);
            targetPos.y = Mathf.Clamp(targetPos.y, _target.position.y - _threshold, _target.position.y + _threshold);

            _cam.transform.position = Vector3.Lerp(_cam.transform.position, targetPos, Time.deltaTime * 5f);
        }
    }
}