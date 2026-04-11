using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : MonoBehaviour, ICollectible
{
    public void Collect(GameObject collector)
    {
        if (collector.TryGetComponent(out PlayerHealth player))
        {
            player.RestoreHealth();
        }
    }
}
