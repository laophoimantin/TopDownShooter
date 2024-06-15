using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPBarManager : MonoBehaviour
{
    [SerializeField] private Image experienceFill;


    private PlayerStats playerStats;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        float currentXP = playerStats.experience;
        float targetXP = playerStats.experienceCap;

       


        experienceFill.fillAmount = currentXP / targetXP;
    }
}
