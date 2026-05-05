using UnityEngine;

public class ComboTracker : MonoBehaviour
{
    public PlayerStats PlayerStatsData;

    private void Awake()
    {
        if (PlayerStatsData == null)
        {
            Debug.LogError("ComboTracker: PlayerStatsData is not assigned in the inspector.");
        }
    }

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;

        HandleDeath.OnEnemyDeath += HandleDeath_OnEnemyDeath;
        PlayerGround.OnGround += PlayerGround_OnGround;

        PlayerStatsData.ResetComboLevel();
        PlayerStatsData.ResetRunBestComboLevel();
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;

        HandleDeath.OnEnemyDeath -= HandleDeath_OnEnemyDeath;
        PlayerGround.OnGround -= PlayerGround_OnGround;

        PlayerStatsData.ResetComboLevel();
        PlayerStatsData.ResetRunBestComboLevel();
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        if (state == HandleGameState.GameState.GameRestart)
        {
            PlayerStatsData.ResetComboLevel();
            PlayerStatsData.ResetRunBestComboLevel();
        }
    }

    private void HandleDeath_OnEnemyDeath()
    {
        PlayerStatsData.IncreaseComboLevel();
        if (PlayerStatsData.GetComboLevel() > PlayerStatsData.GetRunBestComboLevel())
        {
            PlayerStatsData.IncreaseRunBestComboLevel();
        }

        if (PlayerStatsData.GetRunBestComboLevel() > PlayerStatsData.GetLifetimeBestComboLevel())
        {
            PlayerStatsData.IncreaseLifetimeBestComboLevel();
        }
    }

    private void PlayerGround_OnGround(bool onGround)
    {
        if (onGround)
        {
            PlayerStatsData.ResetComboLevel();
        }
    }
}
