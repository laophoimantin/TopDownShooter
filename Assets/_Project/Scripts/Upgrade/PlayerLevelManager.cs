using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : Singleton<PlayerLevelManager>
{
    private int _currentLevel = 1;
    private int _currentXp = 0;
    private int _xpCap;

    public int CurrentLevel => _currentLevel;
    public int CurrentXp => _currentXp;
    public int XpCap => _currentXp;

    public static event Action<int, int> OnXpChange;
    public static event Action<int> OnLevelUp;

    [Serializable]
    public class LevelRange
    {
        public int StartLevel;
        public int EndLevel;
        public int ExperienceCapIncrease;
    }

    [SerializeField] private List<LevelRange> _levelRanges = new();

    void Start()
    {
        if (_levelRanges == null || _levelRanges.Count == 0)
            return;
        _xpCap = _levelRanges[0].ExperienceCapIncrease;
    }

    public void IncreaseExperience(int amount)
    {
        if (amount <= 0) return;

        _currentXp += amount;

        LevelUpChecker();

        OnXpChange?.Invoke(_currentXp, _xpCap);
    }

    private void LevelUpChecker()
    {
        bool didLevelUp = false;

        while (_currentXp >= _xpCap)
        {
            _currentLevel++;
            _currentXp -= _xpCap;

            int capIncrease = 0;
            bool foundRange = false;

            foreach (LevelRange range in _levelRanges)
            {
                if (_currentLevel >= range.StartLevel && _currentLevel <= range.EndLevel)
                {
                    capIncrease = range.ExperienceCapIncrease;
                    foundRange = true;
                    break;
                }
            }

            if (!foundRange && _levelRanges.Count > 0)
            {
                capIncrease = _levelRanges[^1].ExperienceCapIncrease;
            }

            _xpCap += capIncrease;
            didLevelUp = true;
        }

        if (didLevelUp)
        {
            OnLevelUp?.Invoke(_currentLevel);
        }
    }
}