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

    private GameState _currentState = GameState.Gameplay;
    private GameState _previousState;

    public GameState CurrentState => _currentState;
    
    void OnEnable()
    {
        this.Subscribe<OnPlayerDeathStarted>(LoseGameOver);
        this.Subscribe<OnTimeOut>(WinGameOver);
    }

    void OnDisable()
    {
        if (EventDispatcher.Instance != null)
        {
            this.Unsubscribe<OnPlayerDeathStarted>(LoseGameOver);
            this.Unsubscribe<OnTimeOut>(WinGameOver);
        }
    }

    public void ChangeState(GameState newState)
    {
        if (_currentState == newState) return;

        _previousState = _currentState;
        _currentState = newState;
    }

    public void PauseGame()
    {
        if (_currentState == GameState.Gameplay)
        {
            ChangeState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (_currentState == GameState.Paused)
        {
            ChangeState(_previousState);
        }
    }

    private void LoseGameOver(OnPlayerDeathStarted eventData)
    {
        ChangeState(GameState.GameOver);
    }

    private void WinGameOver(OnTimeOut eventData)
    {
        ChangeState(GameState.GameOver);
    }
}