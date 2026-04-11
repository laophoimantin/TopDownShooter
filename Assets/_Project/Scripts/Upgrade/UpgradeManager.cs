using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : Singleton<UpgradeManager>
{
    [SerializeField] private List<UpgradeData> _availableUpgrades = new();

    public List<UpgradeData> GetThreeRandomUpgrades()
    {
        List<UpgradeData> result = new List<UpgradeData>();
        
        if (_availableUpgrades.Count <= 3) return new List<UpgradeData>(_availableUpgrades);

        List<UpgradeData> pool = new List<UpgradeData>(_availableUpgrades);

        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            result.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex); 
        }

        return result;
    }
}
