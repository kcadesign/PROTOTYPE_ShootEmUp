using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCurrencyManager : MonoBehaviour
{
    public UIDocument UIDocument;
    public PlayerStats PlayerStatsData;

    private bool _isDoubleCurrencyActive = false;

    private void OnEnable()
    {
        CollectStar.OnCurrencyCollected += CollectStar_OnCurrencyCollected;
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
        DoubleCurrency1Level.OnDoubleCurrency += DoubleCurrency1Level_OnDoubleCurrency;

        PlayerStatsData.ResetCurrentCurrency();
        PlayerStatsData.ResetRunTotalCurrency();
    }

    private void OnDisable()
    {
        CollectStar.OnCurrencyCollected -= CollectStar_OnCurrencyCollected;
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;
        DoubleCurrency1Level.OnDoubleCurrency -= DoubleCurrency1Level_OnDoubleCurrency;

        PlayerStatsData.ResetCurrentCurrency();
        PlayerStatsData.ResetRunTotalCurrency();
    }

    private void Start()
    {
        PlayerStatsData.ResetCurrentCurrency();
    }

    private void CollectStar_OnCurrencyCollected(int amount)
    {
        int finalAmount = amount;
        if (_isDoubleCurrencyActive)
        {
            finalAmount *= 2;
        }
        PlayerStatsData.AddToCurrentCurrency(finalAmount);
        PlayerStatsData.AddToRunTotalCurrency(finalAmount);
        PlayerStatsData.AddToLifetimeTotalCurrency(finalAmount);
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        //if (state == HandleGameState.GameState.GameRestart)
        //{
        //    PlayerStatsData.ResetCurrentCurrency();
        //    PlayerStatsData.ResetRunTotalCurrency();
        //}
        switch (state)
        {
            case HandleGameState.GameState.PreGameMenu:
                break;
            case HandleGameState.GameState.Transition:
                break;
            case HandleGameState.GameState.Gameplay:
                break;
            case HandleGameState.GameState.LevelStart:
                break;
            case HandleGameState.GameState.GamePaused:
                break;
            case HandleGameState.GameState.Shop:
                break;
            case HandleGameState.GameState.LevelEnd:
                _isDoubleCurrencyActive = false;
                break;
            case HandleGameState.GameState.ChoosePowerup:
                break;
            case HandleGameState.GameState.BossFight:
                break;
            case HandleGameState.GameState.RunEnd:
                break;
            case HandleGameState.GameState.XPTally:
                break;
            case HandleGameState.GameState.GameRestart:
                PlayerStatsData.ResetCurrentCurrency();
                PlayerStatsData.ResetRunTotalCurrency();
                break;
            case HandleGameState.GameState.GameFinished:
                break;
            case HandleGameState.GameState.Credits:
                break;
            default:
                break;
        }
    }

    private void DoubleCurrency1Level_OnDoubleCurrency()
    {
        _isDoubleCurrencyActive = true;
    }



}
