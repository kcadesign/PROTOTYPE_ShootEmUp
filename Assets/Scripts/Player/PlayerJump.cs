using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJump : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction _jump;

    [Header("Components")]
    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    public Animator PlayerAnimator;

    private Vector2 _velocity;

    [Header("Jumping Stats")]
    [Range(2f, 10f)][Tooltip("Maximum jump height")] public float JumpHeight = 7.3f;
    [Range(0.2f, 1.25f)][Tooltip("How long it takes to reach that height before coming back down")] public float TimeToJumpApex;
    [Range(0f, 5f)][Tooltip("Gravity multiplier to apply when going up")] public float AscendingGravity = 1f;
    [Range(1f, 10f)][Tooltip("Gravity multiplier to apply when coming down")] public float DescendingGravity = 6.17f;
    [Range(0, 2)][Tooltip("How many times can you jump in the air?")] public int MaxAirJumps = 0;

    [Header("Options")]
    [Tooltip("Should the character drop when you let go of jump?")] public bool VariablejumpHeight;
    [Range(1f, 10f)][Tooltip("Gravity multiplier when you let go of jump")] public float JumpCutOff;
    [Tooltip("The fastest speed the character can fall")] public float SpeedLimit;
    [Range(0f, 0.3f)][Tooltip("How long should coyote time last?")] public float CoyoteTime = 0.15f;
    [Range(0f, 0.3f)][Tooltip("How far from ground should we cache your jump?")] public float JumpBuffer = 0.15f;

    [Header("Calculations")]
    private float _jumpSpeed;
    private float _defaultGravityScale;
    private float _gravMultiplier;

    [Header("Current State")]
    private bool _canJumpAgain = false;
    private bool _desiredJump;
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

        _defaultGravityScale = 1f;
    }

    private void Update()
    {
        // Check the frame-based press here and set a flag for physics step
        if (_jump != null && _jump.WasPressedThisFrame())
        {
            _desiredJump = true;
            _pressingJump = true;
        }
        else if (_jump != null && _jump.WasReleasedThisFrame())
        {
            _pressingJump = false;
        }

        SetPhysics();

        //Check if we're on ground, using Kit's Ground script
        _onGround = _playerGround.GetOnGround();

        //Jump buffer allows us to queue up a jump, which will play when we next hit the ground
        if (JumpBuffer > 0)
        {
            //Instead of immediately turning off "desireJump", start counting up...
            //All the while, the DoAJump function will repeatedly be fired off
            if (_desiredJump)
            {
                _jumpBufferCounter += Time.deltaTime;

                if (_jumpBufferCounter > JumpBuffer)
                {
                    //If time exceeds the jump buffer, turn off "desireJump"
                    _desiredJump = false;
                    _jumpBufferCounter = 0;
                }
            }
        }

        //If we're not on the ground and we're not currently jumping, that means we've stepped off the edge of a platform.
        //So, start the coyote time counter...
        if (!CurrentlyJumping && !_onGround)
        {
            _coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            //Reset it when we touch the ground, or jump
            _coyoteTimeCounter = 0;
        }
    }

    private void SetPhysics()
    {
        //Determine the character's gravity scale, using the stats provided. Multiply it by a gravMultiplier, used later
        Vector2 newGravity = new Vector2(0, (-2 * JumpHeight) / (TimeToJumpApex * TimeToJumpApex));
        _playerRigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * _gravMultiplier;
    }

    private void FixedUpdate()
    {
        //Get velocity from Rigidbody 
        _velocity = _playerRigidbody.linearVelocity;

        //Keep trying to do a jump, for as long as desiredJump is true
        if (_desiredJump)
        {
            Jump();
            _playerRigidbody.linearVelocity = _velocity;

            //Skip gravity calculations this frame, so currentlyJumping doesn't turn off
            //This makes sure you can't do the coyote time double jump bug
            return;
        }

        CalculateGravity();
    }

    private void CalculateGravity()
    {
        //We change the character's gravity based on her Y direction

        //If Kit is going up...
        if (_playerRigidbody.linearVelocity.y > 0.01f)
        {
            if (_onGround)
            {
                //Don't change it if Kit is stood on something (such as a moving platform)
                _gravMultiplier = _defaultGravityScale;
            }
            else
            {
                //If we're using variable jump height...)
                if (VariablejumpHeight)
                {
                    //Apply upward multiplier if player is rising and holding jump
                    if (_pressingJump && CurrentlyJumping)
                    {
                        _gravMultiplier = AscendingGravity;
                    }
                    //But apply a special downward multiplier if the player lets go of jump
                    else
                    {
                        _gravMultiplier = JumpCutOff;
                    }
                }
                else
                {
                    _gravMultiplier = AscendingGravity;
                }
            }
        }

        //Else if going down...
        else if (_playerRigidbody.linearVelocity.y < -0.01f)
        {

            if (_onGround)
            //Don't change it if Kit is stood on something (such as a moving platform)
            {
                _gravMultiplier = _defaultGravityScale;
            }
            else
            {
                //Otherwise, apply the downward gravity multiplier as Kit comes back to Earth
                _gravMultiplier = DescendingGravity;
            }

        }
        //Else not moving vertically at all
        else
        {
            if (_onGround)
            {
                CurrentlyJumping = false;
            }

            _gravMultiplier = _defaultGravityScale;
        }

        //Set the character's Rigidbody's velocity
        //But clamp the Y variable within the bounds of the speed limit, for the terminal velocity assist option
        _playerRigidbody.linearVelocity = new Vector3(_velocity.x, Mathf.Clamp(_velocity.y, -SpeedLimit, 100));
    }



    private void Jump()
    {
        //_playerRigidbody.AddForceY(JumpForce, ForceMode2D.Impulse);

        //Create the jump, provided we are on the ground, in coyote time, or have a double jump available
        if (_onGround || (_coyoteTimeCounter > 0.03f && _coyoteTimeCounter < CoyoteTime) || _canJumpAgain)
        {
            _desiredJump = false;
            _jumpBufferCounter = 0;
            _coyoteTimeCounter = 0;

            //If we have double jump on, allow us to jump again (but only once)
            _canJumpAgain = (MaxAirJumps == 1 && _canJumpAgain == false);

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
            _desiredJump = false;
        }

        // play jump animation
        PlayerAnimator.SetTrigger("Jump");
    }

    public bool GetJumping() { return CurrentlyJumping; }


}
