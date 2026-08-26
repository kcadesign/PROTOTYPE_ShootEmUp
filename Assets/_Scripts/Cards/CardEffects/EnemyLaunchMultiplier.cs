using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyLaunchMultiplier", menuName = "Scriptable Objects/Cards/Card Effects/EnemyLaunchMultiplier")]
public class EnemyLaunchMultiplier : CardEffect
{
    public static event Action OnMultiplyEnemyLaunch; // Event to notify when the player heals 1 HP

    public override void ApplyEffect()
    {
        Debug.Log("EnemyLaunchMultiplier card effect applied.");
        OnMultiplyEnemyLaunch?.Invoke(); // Trigger the event to notify that the player should multiply their launch multiplier
    }
}
