using System;
using UnityEngine;

public class HandleGameState : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;

    public GameState State;

    public enum GameState
    {
        PreGameMenu,
        Transition,
        Gameplay,
        LevelStart,
        GamePaused,
        Shop,
        LevelEnd,
        ChoosePowerup,
        BossFight,
        RunEnd,
        XPTally,
        GameRestart,
        GameFinished,
        Credits,
    }

    private void Start()
    {
        UpdateGameState(GameState.PreGameMenu);
    }

    private void OnEnable()
    {
        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_OnPlayerEnterLevelEnd;
        SceneController.OnLevelLoaded += SceneController_OnLevelLoaded;
        HandlePlayerDeath.OnPlayerDeath += HandlePlayerDeath_OnPlayerDeath;
        UIController.OnMainMenuButtonPressed += UIController_OnMainMenuButtonPressed;
        UIController.OnRetryButtonPressed += UIController_OnRetryButtonPressed;
        UIController.OnPauseMenuActive += UIController_OnPauseMenuActive;
    }

    private void OnDisable()
    {

        LevelEnd.OnPlayerEnterLevelEnd -= LevelEnd_OnPlayerEnterLevelEnd;
        SceneController.OnLevelLoaded -= SceneController_OnLevelLoaded;
        HandlePlayerDeath.OnPlayerDeath -= HandlePlayerDeath_OnPlayerDeath;
        UIController.OnMainMenuButtonPressed -= UIController_OnMainMenuButtonPressed;
        UIController.OnRetryButtonPressed -= UIController_OnRetryButtonPressed;
        UIController.OnPauseMenuActive -= UIController_OnPauseMenuActive;
    }

    private void LevelEnd_OnPlayerEnterLevelEnd()
    {
        UpdateGameState(GameState.LevelEnd);
        UpdateGameState(GameState.ChoosePowerup);
    }

    private void SceneController_OnLevelLoaded()
    {
        UpdateGameState(GameState.LevelStart);
        UpdateGameState(GameState.Gameplay);
    }

    private void HandlePlayerDeath_OnPlayerDeath()
    {
        UpdateGameState(GameState.RunEnd);
        UpdateGameState(GameState.XPTally);
    }

    private void UIController_OnMainMenuButtonPressed()
    {
        UpdateGameState(GameState.GameRestart);
        UpdateGameState(GameState.PreGameMenu);
    }

    private void UIController_OnRetryButtonPressed()
    {
        UpdateGameState(GameState.GameRestart);
        UpdateGameState(GameState.LevelStart);
        UpdateGameState(GameState.Gameplay);
    }

    private void UIController_OnPauseMenuActive(bool isActive)
    {
        if (isActive)
        {
            UpdateGameState(GameState.GamePaused);
        }
        else
        {
            UpdateGameState(GameState.Gameplay);
        }
    }

    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (State)
        {
            case GameState.PreGameMenu:
                break;
            case GameState.Transition:
                break;
            case GameState.LevelStart:
                break;
            case GameState.Gameplay:
                break;
            case GameState.GamePaused:
                break;
            case GameState.Shop:
                break;
            case GameState.LevelEnd:
                break;
            case GameState.ChoosePowerup:
                break;
            case GameState.BossFight:
                break;
            case GameState.RunEnd:
                break;
            case GameState.XPTally:
                break;
            case GameState.GameRestart:
                break;
            case GameState.GameFinished:
                break;
            case GameState.Credits:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(State), State, null);
        }
        OnGameStateChanged?.Invoke(State);
        PrintState(State);
    }

    private void PrintState(GameState newState)
    {
        Debug.Log($"Current state: {newState}");
    }


    private static void ExitGame()
    {
        // exit play mode or exit application
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
