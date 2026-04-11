using UnityEngine;
using UnityEngine.UI;
using System;

using System.Collections;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
       Gameplay,
       Paused,
       GameOver
    }

    public GameState CurrentState { get; private set; } = GameState.Gameplay;
    private GameState _previousState;

    public static event Action<GameState> OnGameStateChanged;

    void OnEnable()
    {
       PlayerHealth.OnDeathStarted += HandleGameOver;
       CountdownTimer.OnTimeOut += HandleGameOver;
    }
    
    void OnDisable()
    {
       PlayerHealth.OnDeathStarted -= HandleGameOver;
       CountdownTimer.OnTimeOut -= HandleGameOver;
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return; 

        _previousState = CurrentState;
        CurrentState = newState;

        switch (CurrentState)
        {
            case GameState.Gameplay:
                break;
            case GameState.Paused:
	            break;
            case GameState.GameOver:
                break;
        }

        OnGameStateChanged?.Invoke(CurrentState); 
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Gameplay)
        {
            ChangeState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            ChangeState(_previousState); 
        }
    }

    private void HandleGameOver()
    {
        ChangeState(GameState.GameOver);
    }
}


