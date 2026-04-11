using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
    [SerializeField] private GameObject _uiContainer;
    [SerializeField] private List<UpgradeSelectButton> _upgradeSelectButtons;

    private void OnEnable()
    {
        PlayerLevelManager.OnLevelUp += DisplayRandomUpgrades;
    }

    private void OnDisable()
    {
        PlayerLevelManager.OnLevelUp -= DisplayRandomUpgrades;
    }

    void Start()
    {
        HidePanel();
    }
    
    private void DisplayRandomUpgrades(int level)
    {
        GameManager.Instance.PauseGame();
        _uiContainer.SetActive(true);

        List<UpgradeData> randomUpgrades = UpgradeManager.Instance.GetThreeRandomUpgrades();

        for (int i = 0; i < _upgradeSelectButtons.Count; i++)
        {
            if (i < randomUpgrades.Count)
            {
                _upgradeSelectButtons[i].gameObject.SetActive(true);
                _upgradeSelectButtons[i].Setup(randomUpgrades[i], this, UpgradeManager.Instance.Player); 
            }
            else
            {
                _upgradeSelectButtons[i].gameObject.SetActive(false); 
            }
        }
    }

    public void HidePanel()
    {
        _uiContainer.SetActive(false);
        GameManager.Instance.ResumeGame();
    }
}