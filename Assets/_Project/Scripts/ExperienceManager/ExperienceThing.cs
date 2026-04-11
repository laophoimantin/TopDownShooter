using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceThing : MonoBehaviour, ICollectible
{
    public void Collect(GameObject collector)
    {
        PlayerLevelManager.Instance.IncreaseExperience(1);
    }
}

