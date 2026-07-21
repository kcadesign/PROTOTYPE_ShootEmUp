using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardUI CardUIData;

    //private Card _card1;
    //private Card _card2;
    //private Card _card3;

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;
    }

    private void Start()
    {
        CardUIData.InitialiseRunDeck();
        CardUIData.ClearCardSlots();
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        if (state == HandleGameState.GameState.ChoosePowerup)
        {
            // populate cards from deck
            CardUIData.ChooseNewCards();
        }

    }
}
