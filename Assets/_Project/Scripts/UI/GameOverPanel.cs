using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    [Header("UI Slots")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private Image _resultIcon;

    [Header("Resources")]
    [SerializeField] private Sprite _winSprite;
    [SerializeField] private Sprite _loseSprite;
    
    [SerializeField] private Button _quitButton;

    void OnEnable()
    {
        PlayerHealth.OnDeathFinished += () => ShowMenu(false);
        CountdownTimer.OnTimeOut += () => ShowMenu(true);
    }

    void OnDisable()
    {
        PlayerHealth.OnDeathFinished -= () => ShowMenu(false);
        CountdownTimer.OnTimeOut -= () => ShowMenu(true);
    }

    void Start()
    {
        _quitButton.onClick.AddListener(SceneController.Instance.LoadMainMenu);
        _panel.SetActive(false);
    }

    private void ShowMenu(bool isWin)
    {
        _panel.SetActive(true);

        _levelText.text = $"LEVEL: {PlayerLevelManager.Instance.CurrentLevel}";

        var timer = FindObjectOfType<CountdownTimer>();
        float timeSpent = isWin ? timer.TotalTime : timer.TimeElapsed;
        int mins = Mathf.FloorToInt(timeSpent / 60);
        int secs = Mathf.FloorToInt(timeSpent % 60);
        _timeText.text = $"TIME: {mins:00}:{secs:00}";

        _resultIcon.sprite = isWin ? _winSprite : _loseSprite;
    }
}
