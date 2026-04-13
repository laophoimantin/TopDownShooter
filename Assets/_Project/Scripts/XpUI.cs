using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelDisplay;
    [SerializeField] private Image _experienceFill;


    void OnEnable()
    {
        PlayerLevelManager.Instance.OnXpChange += UpdateBarFill;
        PlayerLevelManager.Instance.OnLevelUIChanged += AssignLevelReached;
    }

    void OnDisable()
    {
        if (PlayerLevelManager.Instance != null)
        {
        PlayerLevelManager.Instance.OnXpChange -= UpdateBarFill;
        PlayerLevelManager.Instance.OnLevelUIChanged -= AssignLevelReached; 
        }
    }

    private void AssignLevelReached(int levelReachedData)
    {
        _levelDisplay.text = levelReachedData.ToString();
    }
    
    private void UpdateBarFill(int currentXp, int targetXp)
    {
        if (targetXp <= 0)
        {
            _experienceFill.fillAmount = 0f;
            return; 
        }

        float fillAmount = (float)currentXp / (float)targetXp;
        _experienceFill.fillAmount = Mathf.Clamp01(fillAmount);
    }
}