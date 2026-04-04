using UnityEngine;

public class PlayerEffects : MonoBehaviour
{
    private Jump _playerJump;
    public GameObject AirJumpParticles;

    private void Awake()
    {
        _playerJump = GetComponent<Jump>();
    }

    private void Update()
    {
        if (_playerJump.GetIsAirJumping())
        {
            AirJumpParticles.SetActive(true);
        }
        else
        {
            AirJumpParticles.SetActive(false);
        }
    }

}
