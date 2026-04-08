using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
	[SerializeField] private Button _quitButton;

	void Start()
	{
		_quitButton.onClick.AddListener(QuitDaGame);
	}

	private void QuitDaGame()
	{
		SceneController.Instance.QuitGame();
	}
}