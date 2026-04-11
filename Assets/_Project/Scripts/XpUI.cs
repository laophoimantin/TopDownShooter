using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XpUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelDisplay;
    [SerializeField] private Image _experienceFill;

    void Start()
    {
        if (PlayerLevelManager.Instance != null)
        {
            AssignLevelReached(PlayerLevelManager.Instance.CurrentLevel);
            UpdateBarFill(PlayerLevelManager.Instance.CurrentXp, PlayerLevelManager.Instance.XpCap);
        }
    }

    void OnEnable()
    {
        PlayerLevelManager.OnXpChange += UpdateBarFill;
        PlayerLevelManager.OnLevelUp += AssignLevelReached;
    }

    void OnDisable()
    {
        PlayerLevelManager.OnXpChange -= UpdateBarFill;
        PlayerLevelManager.OnLevelUp -= AssignLevelReached; 
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