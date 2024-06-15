using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointChecker : MonoBehaviour
{
    public Collider2D mapCollider;
    public GameObject[] spawnPoints;

    void Start()
    {
        CheckSpawnPoints();

    }

    void CheckSpawnPoints()
    {
        foreach (GameObject spawnPoint in spawnPoints)
        {
            Collider2D spawnPointCollider = spawnPoint.GetComponent<Collider2D>();
            if (spawnPointCollider != null && mapCollider != null)
            {
                if (mapCollider.bounds.Contains(spawnPointCollider.bounds.center))
                {
                    Debug.Log(spawnPoint.name + " is within the map bounds.");
                }
                else
                {
                    Debug.Log(spawnPoint.name + " is outside the map bounds.");
                    // Disable or move spawnPoint here as needed
                }
            }
        }
    }
}
