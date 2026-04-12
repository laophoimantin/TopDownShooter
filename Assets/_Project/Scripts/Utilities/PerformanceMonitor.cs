using System.Diagnostics;
using TMPro;
using UnityEngine;

public class PerformanceMonitor : Singleton<PerformanceMonitor>
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private TextMeshProUGUI _msText;
    [SerializeField] private TextMeshProUGUI _entityCountText;

    private int _frameCount;
    private float _deltaTime;
    private Stopwatch _stopwatch = new Stopwatch();

    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        float fps = 1.0f / _deltaTime;
        
        _frameCount++;
        if (_frameCount % 10 == 0) 
        {
            _fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
        }
    }

    public void StartLogicTimer()
    {
        _stopwatch.Restart();
    }

    public void StopLogicTimer(int entityCount)
    {
        _stopwatch.Stop();
        
        _msText.text = $"Logic CPU Time: {_stopwatch.Elapsed.TotalMilliseconds:F2} ms";
        _entityCountText.text = $"Entities: {entityCount}";
        
        _msText.color = _stopwatch.Elapsed.TotalMilliseconds > 16f ? Color.red : Color.green;
    }
}