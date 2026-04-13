using System.Collections.Generic;
using UnityEngine;

public class UpgradePanel : MonoBehaviour
{
	[SerializeField] private GameObject _uiContainer;
	[SerializeField] private List<UpgradeSelectButton> _upgradeSelectButtons;

	private int _pendingUpgrades = 0;

	private void OnEnable()
	{
		PlayerLevelManager.Instance.OnLevelUp += HandleLevelUp;
	}

	private void OnDisable()
	{
		if (PlayerLevelManager.Instance != null)
		{
			PlayerLevelManager.Instance.OnLevelUp -= HandleLevelUp;
		}
	}

	void Start()
	{
		_uiContainer.SetActive(false);
	}

	private void HandleLevelUp(int levelsGained)
	{
		_pendingUpgrades += levelsGained;
		if (!_uiContainer.activeSelf)
		{
			ShowNextUpgrade();
		}
	}

	private void ShowNextUpgrade()
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

	public void OnUpgradeSelected()
	{
		_pendingUpgrades--; 

		if (_pendingUpgrades > 0)
		{
			ShowNextUpgrade();
		}
		else
		{
			HidePanel();
		}
	}

	private void HidePanel()
	{
		_uiContainer.SetActive(false);
		GameManager.Instance.ResumeGame();
	}
}       