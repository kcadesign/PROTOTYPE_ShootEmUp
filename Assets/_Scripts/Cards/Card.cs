using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Scriptable Objects/Cards/Card")]
public class Card : ScriptableObject
{
    public Sprite CardImage;
    public string CardText;
    public CardTypeEnum CardType;
    public float EffectValue;
    public bool IsUnique;
    public int UnlockLevel;

    public enum CardTypeEnum
    {
        None,
        ExampleCardType1
    }
}
