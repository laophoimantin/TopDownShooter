using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Transform mCursorVisual;
    public Vector3 mDisplacement;
    void Start()
    {
        // this sets the base cursor as invisible
        Cursor.visible = false;
    }

    void Update()
    {
        mCursorVisual.position = Input.mousePosition + mDisplacement;

    }
}