using UnityEngine;

public class Shield : MonoBehaviour
{
    public GameObject ShieldVisual;

    private void OnEnable()
    {
        PlayerCollisions.OnDamageCollision += PlayerCollisions_OnDamageCollision;
        ShieldPlayer.OnShield += ShieldPlayer_OnShield;
    }

    private void OnDisable()
    {
        PlayerCollisions.OnDamageCollision -= PlayerCollisions_OnDamageCollision;
        ShieldPlayer.OnShield -= ShieldPlayer_OnShield;
    }

    private void PlayerCollisions_OnDamageCollision()
    {
        if (!GetIsShieldActive())
        {
            return;
        }
        SetShieldActive(false);
    }

    private void ShieldPlayer_OnShield()
    {
        SetShieldActive(true);
    }

    public bool GetIsShieldActive()
    {
        return ShieldVisual.activeSelf;
    }

    private void SetShieldActive(bool shieldActive)
    {
        ShieldVisual.SetActive(shieldActive);
    }
}
