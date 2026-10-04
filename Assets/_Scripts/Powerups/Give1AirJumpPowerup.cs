using UnityEngine;

[CreateAssetMenu(fileName = "Give1AirJumpPowerup", menuName = "Scriptable Objects/Powerup/Give1AirJumpPowerup")]
public class Give1AirJumpPowerup : PowerupEffect
{
    public override void Apply(GameObject target)
    {
        target.GetComponent<Stamina>().RestoreStamina(1);
    }
}
