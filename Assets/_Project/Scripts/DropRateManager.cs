using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject itemPrefab;
        public float droprate;
    }
    public List<Drops> drops;


    public void DropItem()
    {
        float randomNumber = Random.Range(0f, 100f);

        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.droprate)
            {
                Instantiate(rate.itemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}