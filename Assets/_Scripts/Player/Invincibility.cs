using UnityEngine;

public class Invincibility : MonoBehaviour
{
    private Jump _playerJump;
    private Dash _playerDash;
    public Grapple PlayerGrapple;

    private bool _isPlayerInvincible = false;

    private void Awake()
    {
        _playerJump = GetComponent<Jump>();
        _playerDash = GetComponent<Dash>();
    }

    private void Update()
    {
        if (_playerJump.GetIsAirJumping() || _playerDash.GetIsDashing() || PlayerGrapple.GetIsGrappling())
        {
            _isPlayerInvincible = true;
        }
        else
        {
            _isPlayerInvincible = false;
        }
    }

    public bool GetPlayerInvincible()
    {
        return _isPlayerInvincible;
    }
}
