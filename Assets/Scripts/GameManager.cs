using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public enum GameState
    {
        Gameplay,
        Paused,
        GameOver,
        LevelUp
    }

    // Store the current state of the game
    public GameState currentState;
    // Store the previous state of the game
    public GameState previousState;



    [Header("Ui")]
    public GameObject pauseScreen;
    public GameObject resultScreen;
    public GameObject levelUpScreen;

    public bool isGameOver = false;
    public bool choosingUpgrade;

    [Header("Results Screen Displays")]
    public Text levelReached;
    public Text timeSurvivedDisplay;

    [Header("StopWatch")]
    private float stopwatchTime;
    public Text stopwatchDisplay;

    [Header("LV Display")]
    public Text LVDisplay;

    [Header("Win")]
    public Sprite happyPenny;
    public Image playerDisplay;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED");
        }
        DisableScreen();
    }

    private void Update()
    {
        // Define the behaviour for each state
        switch (currentState)
        {
            case GameState.Gameplay:
                CheckForPauseAndResume();
                UpdateStopwatch();
                CheckForWin();
                break;

            case GameState.Paused:
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                if (!isGameOver)
                {
                    isGameOver = true;
                    Time.timeScale = 0f;
                    DisplayResults();
                }
                break;
            case GameState.LevelUp:
                if (!choosingUpgrade)
                {
                    choosingUpgrade = true;
                    Time.timeScale = 0f;
                    levelUpScreen.SetActive(true);
                }
                break;

            default:
                Debug.LogWarning("STATE DOES NOT EXIST!");
                break;
        }
    }

    private void CheckForWin()
    {
        if (stopwatchTime >= 5f)
        {
            playerDisplay.sprite = happyPenny;
            playerDisplay.SetNativeSize();
            GameOver();
        }
    }

    // Define the method to change the state of the game
    public void ChangeState(GameState newState)
    {
        currentState = newState;
    }
    public void PauseGame()
    {
        if (currentState != GameState.Paused)
        {
            currentState = GameState.Paused;
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // Stop the game
            pauseScreen.SetActive(true);
            Debug.Log("GAme is paused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            currentState = previousState;
            Time.timeScale = 1f; // Resume the game
            pauseScreen.SetActive(false);
            Debug.Log("Game is resumed");
        }
    }

    void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScreen()
    {
        pauseScreen.SetActive(false);
        resultScreen.SetActive(false);
        levelUpScreen.SetActive(false);
    }

    //public IEnumerator GameOver()
    //{
    //    Debug.Log("GameOver");
    //    yield return new WaitForSeconds(2f);
    //    Debug.Log("GameOver");
    //    timeSurvivedDisplay.text = stopwatchDisplay.text;
    //    yield return new WaitForSeconds(2f);
    //    ChangeState(GameState.GameOver);
    //}

    public void GameOver()
    {
        timeSurvivedDisplay.text = ("Survived: " + stopwatchDisplay.text);
        ChangeState(GameState.GameOver);
    }

    private void DisplayResults()
    {
        resultScreen.SetActive(true);
    }

    public void AssignLevelReached(int levelReachedData)
    {
        levelReached.text =("Level: " + levelReachedData.ToString());
    }

    public void CurrentLVDisplay(int currentLevel)
    {
        LVDisplay.text = currentLevel.ToString();
    }

    private void UpdateStopwatch()
    {
        stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();
    }

    private void UpdateStopwatchDisplay()
    {
        int minutes = Mathf.FloorToInt(stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(stopwatchTime % 60);

        stopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
    }

    public void EndLevelUp()
    {
        choosingUpgrade = false;
        Time.timeScale = 1f;
        levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }

    public void DestroyEverything()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        Time.timeScale = 1f;

        // Loop through all objects and destroy them
        foreach (GameObject obj in allObjects)
        {
            if (obj != Camera.main.gameObject)
            {
                Destroy(obj);
            }
        }
    }
        
}


