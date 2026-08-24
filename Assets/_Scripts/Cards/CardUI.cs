using UnityEngine;
using System.Collections.Generic;
using EditorAttributes;


[CreateAssetMenu(fileName = "New Card UI", menuName = "Scriptable Objects/Cards/Card UI")]
public class CardUI : ScriptableObject
{
    public Card Card1;
    public Card Card2;
    public Card Card3;

    public int Card1Cost;
    public int Card2Cost;
    public int Card3Cost;

    public List<Card> MainDeck; // Main deck that stores every possible card. This deck is not changed at runtime.
    [SerializeField] private List<Card> RunDeck; // Fill this deck at the beginning of each run
    [SerializeField] private List<Card> SelectedCards; // Cards the player has picked so far in the current run

    public void InitialiseRunDeck()
    {
        RunDeck = new List<Card>(MainDeck);
    }

    [Button("Refresh Card Choices")]
    public void ChooseNewCards()
    {
        if (Card1 != null) Destroy(Card1);
        if (Card2 != null) Destroy(Card2);
        if (Card3 != null) Destroy(Card3);

        List<Card> newCardChoices = new List<Card>();

        while (newCardChoices.Count < 3 && GetDeckSize() > 0)
        {
            Card selectedCard = RunDeck[Random.Range(0, RunDeck.Count)]; // choose a random card from the deck
            // remove card from deck to avoid duplicates
            if (selectedCard != null && selectedCard.IsUnique)
            {
                RunDeck.Remove(selectedCard);
            }

            if (!newCardChoices.Contains(selectedCard))
            {
                newCardChoices.Add(selectedCard);
            }
        }
        SetCardInSlot (0, newCardChoices[0]);
        SetCardInSlot (1, newCardChoices[1]);
        SetCardInSlot (2, newCardChoices[2]);
    }

    public int GetDeckSize()
    {
        if (RunDeck.Count < 3)
        {
            Debug.LogWarning("Run Deck has less than 3 cards. Please add more cards to the deck.");
        }
        return RunDeck.Count;
    }

    public void SetCardInSlot(int index, Card card)
    {
        switch (index)
        {
            case 0:
                Card1 = card;
                Card1Cost = card.CardCost;
                break;
            case 1:
                Card2 = card;
                Card2Cost = card.CardCost;
                break;
            case 2:
                Card3 = card;
                Card3Cost = card.CardCost;
                break;
        }
    }

    public void ClearCardSlots()
    {
        Card1 = null;
        Card2 = null;
        Card3 = null;
    }

    public void AddSelectedCard(Card card)
    {
            SelectedCards.Add(card);
    }

    public void ClearSelectedCardList()
    {
        SelectedCards.Clear();
    }
}
