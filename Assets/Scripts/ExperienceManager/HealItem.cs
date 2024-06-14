using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour, ICollectible
{

    public void Collect()
    {
        PlayerStats player = FindObjectOfType<PlayerStats>();
        player.RestoreHealth();
        Destroy(gameObject);
    }
}
