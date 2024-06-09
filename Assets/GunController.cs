using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunController : MonoBehaviour
{
    private Transform hand;
    [SerializeField] private float gunDistance = 1.5f;

    private bool gunFacingRight = true;
    void Start()
    {
        hand = transform.Find("Hand");
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;

        hand.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        hand.position = transform.position + Quaternion.Euler(0, 0, angle) * new Vector3(gunDistance, 0, 0);

        GunFlipHandle(mousePos);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Bang!");
        }
    }

    private void GunFlipHandle(Vector3 mousePos)
    {
        if (mousePos.x < hand.position.x && gunFacingRight)
        {
            GunFlip();
        }
        else if (mousePos.x > hand.position.x && !gunFacingRight)
        {
            GunFlip();
        }
    }

    private void GunFlip()
    {
        gunFacingRight = !gunFacingRight;
        hand.localScale = new Vector3(hand.localScale.x, hand.localScale.y * -1, hand.localScale.z);
    }
}
