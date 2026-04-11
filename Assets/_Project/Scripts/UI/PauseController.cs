using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField] private RectTransform _pausePanel;

    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _quitButton;

    void Awake()
    {
        _returnButton.onClick.AddListener(ReturnButton);
        _quitButton.onClick.AddListener(QuitButton);
        
    }
    void Start()
    {
        _pausePanel.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pausePanel.gameObject.SetActive(true);
            GameManager.Instance.PauseGame();
        }
    }

    private void ReturnButton()
    {
        _pausePanel.gameObject.SetActive(false);
        GameManager.Instance.ResumeGame();
    }

    private void QuitButton()
    {
        SceneController.Instance.LoadMainMenu();
    }
}