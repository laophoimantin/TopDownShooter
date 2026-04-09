using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceThing : MonoBehaviour, ICollectible
{
    public int experienceGranted;

    public void Collect()
    {
        PlayerHealth player = FindObjectOfType<PlayerHealth>();
        player.IncreaseExperience(experienceGranted);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
            Destroy(gameObject);
        }
    }
}

