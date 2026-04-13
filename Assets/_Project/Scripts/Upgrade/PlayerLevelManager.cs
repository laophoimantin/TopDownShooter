using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelManager : Singleton<PlayerLevelManager>
{
	private int _currentLevel = 1;
	private int _currentXp = 0;
	private int _xpCap;

	private readonly int _maxLevelToCache = 100;

	public int CurrentLevel => _currentLevel;

	// ----------------------------------------------------------------
	public event Action<int, int> OnXpChange;
	public event Action<int> OnLevelUIChanged;

	public event Action<int> OnLevelUp;

	// ----------------------------------------------------------------
	[Serializable]
	public class LevelRange
	{
		public int StartLevel;
		public int EndLevel;
		public int ExperienceCapIncrease;
	}

	[SerializeField] private List<LevelRange> _levelRanges = new();

	private int[] _xpCapCache;

	// =============================================================
	void Start()
	{
		if (!InitializeLevelRanges()) return;

		_currentLevel = 1;
		_currentXp = 0;
		_xpCap = GetCapIncreaseForLevel(_currentLevel);

		OnXpChange?.Invoke(_currentXp, _xpCap);
		OnLevelUIChanged?.Invoke(_currentLevel);
	}

	private bool InitializeLevelRanges()
	{
		if (_levelRanges == null || _levelRanges.Count == 0) return false;

		_xpCapCache = new int[_maxLevelToCache + 1];

		for (int i = 1; i <= _maxLevelToCache; i++)
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

	private int GetCapIncreaseForLevel(int level)
	{
		if (level <= _maxLevelToCache)
			return _xpCapCache[level];

		return _levelRanges[^1].ExperienceCapIncrease;
	}

	public void IncreaseExperience(int amount)
	{
		if (amount <= 0) return;

		_currentXp += amount;

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

			_xpCap += GetCapIncreaseForLevel(_currentLevel);
		}

		if (levelsGained > 0)
		{
			OnLevelUIChanged?.Invoke(_currentLevel);

			OnLevelUp?.Invoke(levelsGained);
		}

		OnXpChange?.Invoke(_currentXp, _xpCap);
	}
}