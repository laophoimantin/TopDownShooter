using System;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private float _totalTime = 60f; 
    [SerializeField] private TextMeshProUGUI _timeText;
    private float _currentTime;
    private bool _isTimeUp = false;

    public float TimeElapsed => _totalTime - _currentTime;
    public float TotalTime => _totalTime;
    
    public static event Action OnTimeOut;

    void Start()
    {
        _currentTime = _totalTime;
        UpdateTimerUI(); 
    }

    void Update()
    {
        if (_isTimeUp) return; 

        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0)
        {
            _currentTime = 0;
            _isTimeUp = true;
            
            UpdateTimerUI(); 
            OnTimeOut?.Invoke(); 
        }
        else
        {
            // Còn sống thì còn đếm
            UpdateTimerUI(); 
        }
    }

    private void UpdateTimerUI()
    {
        if (_timeText == null) return;

        int minutes = Mathf.FloorToInt(_currentTime / 60);
        int seconds = Mathf.FloorToInt(_currentTime % 60);

        _timeText.text = $"{minutes:00}:{seconds:00}";
    }
}