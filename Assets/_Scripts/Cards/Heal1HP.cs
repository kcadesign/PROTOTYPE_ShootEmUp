using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal1HPCardEffect", menuName = "Scriptable Objects/Cards/Heal1HPCardEffect")]
public class Heal1HP : CardEffect
{
    public static event Action OnHeal1HP; // Event to notify when the player heals 1 HP

    public override void ApplyEffect()
    {
        Debug.Log("Heal1HP card effect applied.");
        OnHeal1HP?.Invoke(); // Trigger the event to notify that the player should heal 1 HP
    }
}

