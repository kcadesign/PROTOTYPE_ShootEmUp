using UnityEngine;
using UnityEngine.InputSystem;

public class HandlePlayerInput : MonoBehaviour
{
    public InputActionAsset InputActions;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();

        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();

        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState newState)
    {
        switch (newState)
        {
            case HandleGameState.GameState.PreGameMenu:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.Transition:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.LevelStart:
                InputActions.FindActionMap("Player").Enable();
                break;
            case HandleGameState.GameState.Gameplay:
                InputActions.FindActionMap("Player").Enable();
                break;
            case HandleGameState.GameState.GamePaused:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.Shop:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.LevelEnd:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.ChoosePowerup:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.BossFight:
                InputActions.FindActionMap("Player").Enable();
                break;
            case HandleGameState.GameState.RunEnd:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.XPTally:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.GameFinished:
                InputActions.FindActionMap("Player").Disable();
                break;
            case HandleGameState.GameState.Credits:
                InputActions.FindActionMap("Player").Disable();
                break;
        }
    }


}
