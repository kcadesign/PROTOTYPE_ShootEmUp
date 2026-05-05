using UnityEngine;

public class LevelTracker : MonoBehaviour
{
    public PlayerStats PlayerStatsData;

    private void Awake()
    {
        if (PlayerStatsData == null)
        {
            Debug.LogError("PlayerStatsData is not assigned in the inspector.");
        }
    }

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;

        PlayerStatsData.ResetRunHighestLevel();
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;

        PlayerStatsData.ResetRunHighestLevel();
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        if (state == HandleGameState.GameState.LevelStart)
        {
            PlayerStatsData.IncreaseRunHighestLevel();
            if(PlayerStatsData.GetRunHighestLevel() > PlayerStatsData.GetLifetimeHighestLevel()) 
            {
                PlayerStatsData.SetLifetimeHighestLevel(PlayerStatsData.GetRunHighestLevel());
            }
        }
        else if (state == HandleGameState.GameState.GameRestart)
        {
            PlayerStatsData.ResetRunHighestLevel();
        }
    }
}
