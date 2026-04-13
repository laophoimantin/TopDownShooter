using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : Singleton<PlayerLevelManager>
{
    private int _currentLevel = 1;
    private int _currentXp = 0;
    private int _xpCap;

    public int CurrentLevel => _currentLevel;

    // ----------------------------------------------------------------
    public event Action<int, int> OnXpChange;
    public event Action<int> OnLevelUIChanged;
    public event Action OnLevelUp;

    // ----------------------------------------------------------------
    [Serializable]
    public class LevelRange
    {
        public int StartLevel;
        public int EndLevel;
        public int ExperienceCapIncrease;
    }

    [SerializeField] private List<LevelRange> _levelRanges = new();
    private Dictionary<int, int> _xpCapCache = new();

    // =============================================================
    void Start()
    {
        if (!InitializeLevelRanges()) return;

        _currentLevel = 1;
        _currentXp = 0;

        _xpCap = _xpCapCache.TryGetValue(_currentLevel, out int cap) ? cap : _levelRanges[^1].ExperienceCapIncrease;

        OnXpChange?.Invoke(_currentXp, _xpCap);
        OnLevelUIChanged?.Invoke(_currentLevel);
    }

    private bool InitializeLevelRanges()
    {
        if (_levelRanges == null || _levelRanges.Count == 0)
        {
            return false;
        }

        int maxLevelToCache = 100;
        for (int i = 1; i <= maxLevelToCache; i++)
        {
            _xpCapCache[i] = CalculateCapIncreaseForLevel(i);
        }

        return true;
    }

    private int CalculateCapIncreaseForLevel(int level)
    {
        foreach (var range in _levelRanges)
        {
            if (level >= range.StartLevel && level <= range.EndLevel)
                return range.ExperienceCapIncrease;
        }

        return _levelRanges[^1].ExperienceCapIncrease;
    }

    public void IncreaseExperience(int amount)
    {
        if (amount <= 0) return;

        _currentXp += amount;
        OnXpChange?.Invoke(_currentXp, _xpCap);
        LevelUpChecker();
    }

    private void LevelUpChecker()
    {
        int levelsGained = 0;

        while (_currentXp >= _xpCap)
        {
            _currentXp -= _xpCap;
            _currentLevel++;
            levelsGained++;

            _xpCap += _xpCapCache.TryGetValue(_currentLevel, out int cap) ? cap : _levelRanges[^1].ExperienceCapIncrease;
        }

        if (levelsGained > 0)
        {
            OnLevelUIChanged?.Invoke(_currentLevel); 
            OnLevelUp?.Invoke();
            OnXpChange?.Invoke(_currentXp, _xpCap);
        }
    }
}