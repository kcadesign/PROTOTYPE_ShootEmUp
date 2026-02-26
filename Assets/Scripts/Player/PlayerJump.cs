using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction _jump;

    [Header("Components")]
    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    public Grapple Grapple;
    private WallJump _wallJump;
    public Animator PlayerAnimator;

    private Vector2 _velocity;

    [Header("Jumping Stats")]
    [Range(2f, 10f)] public float JumpHeight = 7.3f;
    [Range(0.2f, 1.25f)] public float TimeToJumpApex;
    [Range(0f, 5f)] public float AscendingGravity = 1f;
    [Range(1f, 10f)] public float DescendingGravity = 6.17f;
    [Range(0, 2)] public int MaxAirJumps = 0;

    [Header("Options")]
    public bool VariablejumpHeight;
    public float SpeedLimit;
    [Range(0f, 0.3f)] public float CoyoteTime = 0.15f;
    [Range(0f, 0.3f)] public float JumpBuffer = 0.15f;

    [Header("Calculations")]
    private float _jumpSpeed;
    private float _defaultGravityScale;
    private float _gravMultiplier;

    [Header("Current State")]
    private bool _canJumpAgain = false;
    private bool _desireJump;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter = 0;
    private bool _pressingJump;
    private bool _onGround;
    public bool CurrentlyJumping;


    private void Awake()
    {
        _jump = InputActions.FindAction("Jump");
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
        _wallJump = GetComponent<WallJump>();

        _defaultGravityScale = 1f;
    }

    private void Update()
    {
        _onGround = _playerGround.GetOnGround();
        SetPhysics();

        CheckJumpPressed();

        if (JumpBuffer > 0 && _desireJump)
        {
            _jumpBufferCounter += Time.deltaTime;

            if (_jumpBufferCounter > JumpBuffer)
            {
                //If time exceeds the jump buffer, turn off "desireJump"
                _desireJump = false;
                _jumpBufferCounter = 0;
            }
        }

        HandleCoyoteTime();
    }

    private void FixedUpdate()
    {
        //Get velocity from Rigidbody 
        _velocity = _playerRigidbody.linearVelocity;

        //Keep trying to do a jump, for as long as desiredJump is true
        if (_desireJump)
        {
            Jump();
            _playerRigidbody.linearVelocity = _velocity;

            //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
            //This makes sure you can't do the coyote time double jump bug
            return;
        }

        CalculateGravity();
    }

    private void CheckJumpPressed()
    {
        if (_jump != null && _jump.WasPressedThisFrame())
        {
            _desireJump = true;
            _pressingJump = true;
        }
        else if (_jump != null && _jump.WasReleasedThisFrame())
        {
            _pressingJump = false;
        }
    }

    private void HandleCoyoteTime()
    {
        if (!CurrentlyJumping && !_onGround)
        {
            _coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            _coyoteTimeCounter = 0;
        }
    }

    private void SetPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * JumpHeight) / (TimeToJumpApex * TimeToJumpApex));
        _playerRigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * _gravMultiplier;
    }

    private void CalculateGravity()
    {
        // small threshold to ignore tiny floating-point fluctuations
        const float errorThreshold = 0.01f;

        // use the cached velocity set in FixedUpdate for consistent reads/writes
        float velocityY = _velocity.y;

        if (velocityY > errorThreshold && _pressingJump)
        {
            // moving up
            _gravMultiplier = AscendingGravity;
        }
        else if (velocityY < -errorThreshold)
        {
            // falling
            _gravMultiplier = DescendingGravity;
        }
        else
        {
            // near zero vertical speed
            if (_onGround && Mathf.Abs(velocityY) <= errorThreshold)
            {
                // on ground and not moving -> default gravity and end jumping state
                _gravMultiplier = _defaultGravityScale;
                CurrentlyJumping = false;
            }
            else
            {
                // at apex in mid-air -> start falling
                _gravMultiplier = DescendingGravity;
            }
        }

        // Apply clamped velocity back to the Rigidbody (preserves horizontal velocity)
        _playerRigidbody.linearVelocity = new Vector3(_velocity.x, Mathf.Clamp(_velocity.y, -SpeedLimit, 100));
    }

    private void Jump()
    {
        //_playerRigidbody.AddForceY(JumpForce, ForceMode2D.Impulse);

        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (_onGround || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < CoyoteTime) || _canJumpAgain)
        {
            _desireJump = false;
            _jumpBufferCounter = 0;
            _coyoteTimeCounter = 0;

            //If we have double jump on, allow us to jump again (but only once) and reset our jump count after grappling
            _canJumpAgain = (MaxAirJumps == 1 && !_canJumpAgain && !_wallJump.GetOnWall());

            //Determine the power of the jump, based on our gravity and stats
            _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _playerRigidbody.gravityScale * JumpHeight);

            //If Kit is moving up or down when she jumps (such as when doing a double jump), change the jumpSpeed;
            //This will ensure the jump is the exact same strength, no matter your velocity.
            if (_velocity.y > 0f)
            {
                _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                _jumpSpeed += Mathf.Abs(_playerRigidbody.linearVelocity.y);
            }

            //Apply the new jumpSpeed to the velocity. It will be sent to the Rigidbody in FixedUpdate;
            _velocity.y += _jumpSpeed;
            CurrentlyJumping = true;
        }

        if (JumpBuffer == 0)
        {
            //If we don't have a jump buffer, then turn off desiredJump immediately after hitting jumping
            _desireJump = false;
        }

        // play jump animation
        PlayerAnimator.SetTrigger("Jump");
    }

    // Public API to allow external systems (like Grapple) to reset the air-jump availability.
    public void ResetAirJump()
    {
        if (MaxAirJumps == 1)
        {
            _canJumpAgain = true;
        }
    }

    public bool GetJumping() { return CurrentlyJumping; }


}
