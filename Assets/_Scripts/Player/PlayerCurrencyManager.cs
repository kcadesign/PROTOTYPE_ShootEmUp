using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCurrencyManager : MonoBehaviour
{
    public UIDocument UIDocument;
    public PlayerStats PlayerStatsData;

    private void OnEnable()
    {
        CollectStar.OnCurrencyCollected += CollectStar_OnCurrencyCollected;
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;

        PlayerStatsData.ResetCurrentCurrency();
        PlayerStatsData.ResetRunTotalCurrency();
    }

    private void OnDisable()
    {
        CollectStar.OnCurrencyCollected -= CollectStar_OnCurrencyCollected;
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;

        PlayerStatsData.ResetCurrentCurrency();
        PlayerStatsData.ResetRunTotalCurrency();
    }

    private void Start()
    {
        PlayerStatsData.ResetCurrentCurrency();
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        if (state == HandleGameState.GameState.GameRestart)
        {
            PlayerStatsData.ResetCurrentCurrency();
            PlayerStatsData.ResetRunTotalCurrency();
        }
    }

    private void CollectStar_OnCurrencyCollected(int amount)
    {
        PlayerStatsData.AddToCurrentCurrency(amount);
        PlayerStatsData.AddToRunTotalCurrency(amount);
        PlayerStatsData.AddToLifetimeTotalCurrency(amount);
    }

}
