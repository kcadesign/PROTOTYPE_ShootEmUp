using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Cards/Card")]
public class Card : ScriptableObject
{
    public Sprite CardImage;
    public Sprite CardBackground;
    public string CardDescription;
    public string CardCost;
    public CardTypeEnum CardType;
    public float EffectValue;
    public bool IsUnique;
    public int UnlockLevel;

    [Header("Card Effect")]
    public UnityEvent OnCardSelected;

    public enum CardTypeEnum
    {
        None,
        ExampleCardType1,
        ExampleCardType2,
        ExampleCardType3,
        Heal
    }

    public void ActivateCardEffect()
    {
        OnCardSelected?.Invoke();
        Debug.Log("Running card effect for: " + name);
    }
}
