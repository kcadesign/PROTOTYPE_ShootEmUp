using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DoubleCurrency1LevelEffect", menuName = "Scriptable Objects/Cards/Card Effects/DoubleCurrency1LevelEffect")]
public class DoubleCurrency1Level : CardEffect
{
    public static event Action OnDoubleCurrency; // Event to notify when the player heals 1 HP

    public override void ApplyEffect()
    {
        Debug.Log("Heal1HP card effect applied.");
        OnDoubleCurrency?.Invoke();
    }
}
