using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ShieldCardEffect", menuName = "Scriptable Objects/Cards/Card Effects/ShieldCardEffect")]
public class ShieldPlayer : CardEffect
{
    public static event Action OnShield; // Event to notify when the player uses double boost jump

    public override void ApplyEffect()
    {
        Debug.Log("Shield card effect applied.");
        OnShield?.Invoke();
    }
}
