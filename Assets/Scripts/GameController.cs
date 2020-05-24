using System;
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

    public static event Action<GameState> OnGameStateChanged = delegate { };

    public GameState currentGameState = GameState.InGame;


    /// Dependencies
    private ScoreAction scoreAction;

    private Timer timer;
    private UiAction uiAction;

    /// Key Check Functions
    private readonly Func<bool> pauseKeyCheck = () => Input.GetKeyDown(P) || Input.GetKeyDown(Escape);

    /// GameObjects
    public GameObject gameEnvironment;
    private readonly Func<bool> muteKeyCheck = () => Input.GetKeyDown(M);

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
        
        // Mute Toggle
        if (muteKeyCheck()) SoundManager.Instance.Mute();
        if (!timer.isTimeOver())
        {
            if (!timer.isPaused())
            {
                // Game is running
                
                // Add bonus time gained by streaks
                timer.addTime(scoreAction.getTimeDelta());
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

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShoppingList()
    {
        SetGameState(GameState.SHOW_SHOPPING_LIST);
    }

    // Used from UI - must be public
    // ReSharper disable once MemberCanBePrivate.Global
    public void StartGame()
    {
        Debug.Log("Start clicked");
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
    public void GameOver()
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
            case GameState.SHOW_SHOPPING_LIST:
                uiAction.ShowShoppingList();
                gameEnvironment.gameObject.SetActive(false);
                timer.pauseTimer();
                break;
            case GameState.InGame:
                uiAction.ShowInGame();
                gameEnvironment.gameObject.SetActive(true);
                timer.resumeTimer();
                break;
            case GameState.GameOver:
                if (currentGameState != newGameState)
                {
                    gameEnvironment.gameObject.SetActive(false);
                    uiAction.ShowGameOver();
                    scoreAction.gameOverLeaderBoard();
                    timer.pauseTimer();
                }
                break;
            case GameState.Pause:
                uiAction.ShowPause();
                timer.pauseTimer();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }

        currentGameState = newGameState;
        OnGameStateChanged(currentGameState);
    }

   
}

/// <summary>
/// Possible states of the game.
/// </summary>
public enum GameState
{
    ///  Game currently displays the shopping list
    SHOW_SHOPPING_LIST,
    
    /// Game currently running.
    InGame,

    /// Game currently displays the game-over canvas.
    GameOver,

    /// Game currently paused
    Pause
}

