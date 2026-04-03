using UnityEngine;

[CreateAssetMenu(fileName = "ActivateAirJumpsPowerup", menuName = "Scriptable Objects/Powerup/ActivateAirJumpsPowerup")]
public class ActivateAirJumpsPowerup : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<Jump>().SetAllowAirJumps(true);
    }
}
