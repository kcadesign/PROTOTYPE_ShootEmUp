using System;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class HandleGameState : MonoBehaviour
{
    public static event Action<GameState> OnGameStateChanged;

    public GameState State;

    public enum GameState
    {
        PreGameMenu,
        Transition,
        Gameplay,
        GamePaused,
        LevelEnd,
        XPTally,
        BossFight,
        GameOver,
        GameFinished,
        Credits,
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
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
            case GameState.Gameplay:
                break;
            case GameState.GamePaused:
                break;
            case GameState.LevelEnd:
                break;
            case GameState.XPTally:
                break;
            case GameState.BossFight:
                break;
            case GameState.GameOver:
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
