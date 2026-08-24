using UnityEngine;
using System;

[CreateAssetMenu(fileName = "DoubleBoostJumpCardEffect", menuName = "Scriptable Objects/Cards/DoubleBoostJumpCardEffect")]
public class DoubleBoostJump : CardEffect
{
    public static event Action OnDoubleBoostJump; // Event to notify when the player uses double boost jump

    public override void ApplyEffect()
    {
        Debug.Log("DoubleBoostJump card effect applied.");
        OnDoubleBoostJump?.Invoke();
    }
}
