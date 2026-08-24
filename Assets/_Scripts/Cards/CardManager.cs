using UnityEngine;

public class CardManager : MonoBehaviour
{
    public CardUI CardUIData;

    private Card _card1;
    private Card _card2;
    private Card _card3;

    private int _card1Cost;
    private int _card2Cost;
    private int _card3Cost;

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;

        UIController.OnCardSelected += UIController_OnCardSelected;
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;

        UIController.OnCardSelected -= UIController_OnCardSelected;


    }

    private void Start()
    {
        CardUIData.InitialiseRunDeck();
        CardUIData.ClearCardSlots();
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        switch (state)
        {
            case HandleGameState.GameState.PreGameMenu:
                CardUIData.ClearSelectedCardList();
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
                break;
            case HandleGameState.GameState.ChoosePowerup:
                //ResetCardCosts();
                CardUIData.ChooseNewCards(); // populate cards from deck
                GetCardSelection();
                //GetCardCosts();
                break;
            case HandleGameState.GameState.BossFight:
                break;
            case HandleGameState.GameState.RunEnd:
                break;
            case HandleGameState.GameState.XPTally:
                break;
            case HandleGameState.GameState.GameRestart:
                CardUIData.ClearSelectedCardList();
                break;
            case HandleGameState.GameState.GameFinished:
                break;
            case HandleGameState.GameState.Credits:
                break;
            default:
                break;
        }

    }

    private void UIController_OnCardSelected(int cardNumber)
    {
        switch (cardNumber)
        {
            case 0:
                Debug.Log($"{(_card1 != null ? _card1.name : "None")} selected");
                // add card to selected cards list
                CardUIData.AddSelectedCard(_card1);
                // run card 1 powerup logic
                _card1.ActivateCardEffect();
                break;
            case 1:
                Debug.Log($"{(_card2 != null ? _card2.name : "None")} selected");
                // add card to selected cards list
                CardUIData.AddSelectedCard(_card2);
                // run card 2 powerup logic
                _card2.ActivateCardEffect();
                break;
            case 2:
                Debug.Log($"{(_card3 != null ? _card3.name : "None")} selected");
                // add card to selected cards list
                CardUIData.AddSelectedCard(_card3);
                // run card 3 powerup logic
                _card3.ActivateCardEffect();
                break;
            default:
                break;
        }
    }

    private void GetCardSelection()
    {
        _card1 = CardUIData.Card1;
        _card2 = CardUIData.Card2;
        _card3 = CardUIData.Card3;
        Debug.Log("Card 1: " + (_card1 != null ? _card1.name : "None"));
        Debug.Log("Card 2: " + (_card2 != null ? _card2.name : "None"));
        Debug.Log("Card 3: " + (_card3 != null ? _card3.name : "None"));
    }

    private void GetCardCosts()
    {
        _card1Cost = _card1.CardCost;
        _card2Cost = _card2.CardCost;
        _card3Cost = _card3.CardCost;
    }

    private void ResetCardCosts()
    {
        _card1Cost = 0;
        _card2Cost = 0;
        _card3Cost = 0;
    }
}
