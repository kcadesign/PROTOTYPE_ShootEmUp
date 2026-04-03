using System;
using UnityEngine;

public class AirJump : MonoBehaviour
{
    public static event Action<bool> OnAirJump;
    public static event Action<int> OnCurrentAirJumpAmountChanged;
    public static event Action<int> OnMaxAirJumpsChanged;

    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    public Animator PlayerAnimator;

    public float JumpHeight = 5.75f;
    private float _defaultGravityScale;

    public float AirJumpMultiplier = 1.1f;
    public bool AllowAirJumps;
    private int _airJumps = 0;
    public int MaxAirJumps = 1;
    private bool _AirJumping;

    private bool _onGround;

    private void Awake()
    {
        _playerGround = GetComponent<PlayerGround>();

    }
    private void Start()
    {
        if (AllowAirJumps)
        {
            _airJumps = MaxAirJumps;
            OnCurrentAirJumpAmountChanged?.Invoke(_airJumps); // Notify initial health
            OnMaxAirJumpsChanged?.Invoke(MaxAirJumps); // Notify initial max health
        }
    }

    private void Update()
    {
        _onGround = _playerGround.GetOnGround();

        if (_onGround && AllowAirJumps)
        {
            _AirJumping = false;
            OnAirJump?.Invoke(false);
            ResetAirJumps();
        }

    }

    public void DoAirJump(float jumpPowerMultiplier)
    {
        // Compute using the engine gravity and the default gravityScale so buffered jumps
        // don't inherit a high "falling" gravityScale and become overpowered.
        float gravity = Physics2D.gravity.y * _playerRigidbody.gravityScale; // negative
        float jumpPower = Mathf.Sqrt(-2f * gravity * JumpHeight); // v = sqrt(2 * |g| * h)

        // zero vertical velocity then apply jump
        _playerRigidbody.linearVelocityY = 0f;
        _playerRigidbody.linearVelocityY = jumpPower * jumpPowerMultiplier;

        // ensure gravityScale is the default while starting the jump
        _playerRigidbody.gravityScale = _defaultGravityScale;
        _AirJumping = true;

        PlayerAnimator.SetTrigger("Jump");

    }


    public void ResetAirJumps()
    {
        _airJumps = MaxAirJumps;
        OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);
    }

}
