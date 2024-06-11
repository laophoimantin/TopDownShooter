using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public int playerHealth = 3;


    void Start()
    {
        
    }

    void Update()
    {
        if(playerHealth <= 0)
        {
            Debug.Log("Dead!");
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Mob"))
        {
            playerHealth -= 1;
            Debug.Log(playerHealth);
        }
    }
}
