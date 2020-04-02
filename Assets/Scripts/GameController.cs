﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.KeyCode;

public class GameController : MonoBehaviour
{
    /// <summary>
    /// Use the instance to test changes via GameController.Instance
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static GameController Instance;

    public GameState currentGameState = GameState.Menu;


    /// Dependencies
    private ScoreAction scoreAction;

    private Timer timer;
    private UiAction uiAction;

    /// Key Check Functions
    private readonly Func<bool> pauseKeyCheck = () => Input.GetKeyDown(P) || Input.GetKeyDown(Escape);

    
    private void Awake()
    {
        Instance = this;
        // Set your dependencies in start to avoid NullPointers!
    }

    /// <summary>
    /// Setup of dependencies and initial state.
    /// </summary>
    private void Start()
    {
        // Dependencies:
        timer = TimerImpl.Instance;
        uiAction = UiController.Instance;
        scoreAction = ScoreController.Instance;

        // Setup:
        // Register consumer of time updates:
        timer.addTimeConsumer(scoreAction.nextTime);
        //timer.addTimeConsumer(time => Debug.Log("Time left: " + time));

        // Set initial game state and start the game implicitly.
        SetGameState(currentGameState);
        // Debug.Log("GameController.Start()");
    }

    void Update()
    {
        if (!timer.isTimeOver())
        {
            // Game is active
            if (!timer.isPaused())
            {
                // Game is running
                
                
                // Pause Key Check
                if (pauseKeyCheck()) PauseGame();
            }
            else
            {
                // Game is paused
                
                
                // Pause Key Check
                if (pauseKeyCheck()) StartGame();
            }
        }
        else
        {
            // Game is over
            GameOver();
        }
    }

    // Used from UI - must be public
    // ReSharper disable once MemberCanBePrivate.Global
    public void StartGame()
    {
        SetGameState(GameState.InGame);
    }

    // Used from UI - must be public
    // ReSharper disable once MemberCanBePrivate.Global
    public void PauseGame()
    {
        SetGameState(GameState.Pause);
    }

    // Used from UI - must be public
    // ReSharper disable once MemberCanBePrivate.Global
    public void BackToMenu()
    {
        SetGameState(GameState.Menu);
    }

    // Used from UI - must be public
    // ReSharper disable once MemberCanBePrivate.Global
    private void GameOver()
    {
        SetGameState(GameState.GameOver);
    }

    // Used from UI - must be public
    // ReSharper disable once MemberCanBePrivate.Global
    /// <summary>
    /// Restarts the game. Resets the scene hard.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetGameState(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Menu:
                uiAction.ShowMenu();
                break;
            case GameState.InGame:
                uiAction.ShowInGame();
                timer.resumeTimer();
                break;
            case GameState.GameOver:
                uiAction.ShowGameOver();
                break;
            case GameState.Pause:
                uiAction.ShowPause();
                timer.pauseTimer();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }

        currentGameState = newGameState;
    }
}

/// <summary>
/// Possible states of the game.
/// </summary>
public enum GameState
{
    /// Game currently displays the menu canvas. 
    Menu,

    /// Game currently running.
    InGame,

    /// Game currently displays the game-over canvas.
    GameOver,

    /// Game currently paused
    Pause
}