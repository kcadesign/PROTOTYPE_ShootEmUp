using UnityEngine;

//[CreateAssetMenu(fileName = "Powerup", menuName = "Scriptable Objects/Powerup")]
public abstract class PowerupEffect : ScriptableObject
{
    public abstract void Apply(GameObject target);
}
