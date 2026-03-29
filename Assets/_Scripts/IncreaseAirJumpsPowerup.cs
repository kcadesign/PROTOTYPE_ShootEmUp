using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseAirJumpsPowerup", menuName = "Scriptable Objects/Powerup/IncreaseAirJumpsPowerup")]
public class IncreaseAirJumpsPowerup : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<Jump>().IncreaseMaxAirJumps();
    }
}
