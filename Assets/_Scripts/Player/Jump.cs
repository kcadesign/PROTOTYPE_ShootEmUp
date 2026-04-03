using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    public static event Action<bool> OnAirJump;
    public static event Action<bool> OnPlayerDescending;

    public static event Action<int> OnCurrentAirJumpAmountChanged;
    public static event Action<int> OnMaxAirJumpsChanged;


    [Header("Input References")]
    private HandlePlayerInput _handlePlayerInput;
    private InputActionAsset _inputActions;
    private InputAction _jump;

    [Header("Component References")]
    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    public Grapple Grapple;
    private WallJump _wallJump;
    public Animator PlayerAnimator;

    [Header("Jump Stats")]
    //public float JumpForce;
    public float JumpHeight = 7.3f;
    //private float _jumpSpeed;
    public float TimeToJumpApex;
    public float AirJumpMultiplier = 1.1f;

    public float AscendingGravity = 1f;
    public float DescendingGravity = 6.17f;
    private float _gravMultiplier;

    [Header("Optionals")]
    public bool AllowAirJumps;
    private int _airJumps = 0;
    public int MaxAirJumps = 1;
    //public bool VariableJumpHeight;

    [Header("Buffers")]
    public float CoyoteTime = 0.15f;
    private float _coyoteTimer = 0;
    public float JumpBuffer = 0.15f;
    private float _jumpBufferTimer;

    [Header("Defaults & Limits")]
    private float _defaultGravityScale;
    public float SpeedLimit;

    [Header("Current State")]
    private bool _pressingJump;
    private bool _desireJump;
    //private bool _canJump = false;
    public bool CurrentlyJumping;
    private bool _AirJumping;
    private bool _isDescending;
    private bool _onGround;

    private void Awake()
    {
        _handlePlayerInput = GetComponent<HandlePlayerInput>();
        _inputActions = _handlePlayerInput.InputActions;
        _jump = _inputActions.FindAction("Jump");

        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
        _wallJump = GetComponent<WallJump>();

        _defaultGravityScale = _playerRigidbody.gravityScale;

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

    void Update()
    {
        _onGround = _playerGround.GetOnGround();

        CheckDescending();

        //ResetAirJumps();
        if (_onGround && AllowAirJumps)
        {
            _AirJumping = false;
            OnAirJump?.Invoke(false);
            ResetAirJumps();
        }

        CheckJumpPressed();

        HandleJumpBuffer();

        HandleCoyoteTime();

        //SetPhysics();

        LimitFallSpeed();
    }

    private void CheckDescending()
    {
        if (_playerRigidbody.linearVelocityY < 0)
        {
            _isDescending = true;
            OnPlayerDescending?.Invoke(_isDescending);
        }
        else
        {
            _isDescending = false;
            OnPlayerDescending?.Invoke(_isDescending);
        }
    }

    private void SetPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * JumpHeight) / (TimeToJumpApex * TimeToJumpApex));
        _playerRigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * _gravMultiplier;
    }


    private void FixedUpdate()
    {
        CheckCanJump();

        CalculateGravity();
    }

    private void CheckJumpPressed()
    {
        if (_jump != null && _jump.WasPressedThisFrame())
        {
            _desireJump = true;
            _pressingJump = true;
            _jumpBufferTimer = JumpBuffer;

            // instantiate a 2D object to show where the player pressed jump for testing purposes
            GameObject jumpPressIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            jumpPressIndicator.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            jumpPressIndicator.transform.position = transform.position;
            Destroy(jumpPressIndicator, 0.5f);
        }
        else if (_jump != null && _jump.WasReleasedThisFrame())
        {
            _pressingJump = false;
            _coyoteTimer = 0;
        }
    }

    private void HandleJumpBuffer()
    {
        _jumpBufferTimer -= Time.deltaTime;
        //Debug.Log($"Jump Buffer Timer: {_jumpBufferTimer}");

        if (_jumpBufferTimer < 0f)
        {
            //If time exceeds the jump buffer, turn off "desireJump"
            _desireJump = false;
            _jumpBufferTimer = 0;
        }
    }

    private void HandleCoyoteTime()
    {
        if (_onGround)
        {
            _coyoteTimer = CoyoteTime;
        }
        if (!CurrentlyJumping && !_onGround)
        {
            _coyoteTimer -= Time.deltaTime;
        }

    }
    private void CalculateGravity()
    {
        // small threshold to ignore tiny floating-point fluctuations
        const float errorThreshold = 0.01f;

        if (_playerRigidbody.linearVelocityY > errorThreshold && _pressingJump)
        {
            // moving up
            _gravMultiplier = AscendingGravity;
        }
        else if (_playerRigidbody.linearVelocityY < -errorThreshold)
        {
            // falling
            _gravMultiplier = DescendingGravity;
        }
        else
        {
            // near zero vertical speed
            if (_onGround && Mathf.Abs(_playerRigidbody.linearVelocityY) <= errorThreshold)
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
        _playerRigidbody.gravityScale = _gravMultiplier;
    }

    private void CheckCanJump()
    {
        if (!_desireJump) return;
        // Don't allow the regular DoJump to run while wall-sliding or during a wall-jump
        if (_wallJump != null && (_wallJump.GetIsWallSLiding() || _wallJump.GetIsWallJumping())) return;

        // allow jump when on ground, during coyote time, or if air jumps remain
        if (_onGround || _coyoteTimer > 0f || _airJumps > 0)
        {
            CurrentlyJumping = true;
            if (!_onGround && AllowAirJumps && !Grapple.GetIsGrappling())
            {
                _AirJumping = true;
                OnAirJump?.Invoke(true);
                _airJumps--;
                OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);
                _desireJump = false;

                DoJump(AirJumpMultiplier);
                _jumpBufferTimer = 0;
                _coyoteTimer = 0;
                return;
            }

            _desireJump = false;

            DoJump();
            _jumpBufferTimer = 0;
            _coyoteTimer = 0;
        }
    }

    public void ResetAirJumps()
    {
        _airJumps = MaxAirJumps;
        OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);
    }

    public void DoJump()
    {
        //Debug.Log("Jumping from jump script");
        // Compute using the engine gravity and the default gravityScale so buffered jumps
        // don't inherit a high "falling" gravityScale and become overpowered.
        float gravity = Physics2D.gravity.y * _defaultGravityScale; // negative
        float jumpPower = Mathf.Sqrt(-2f * gravity * JumpHeight); // v = sqrt(2 * |g| * h)

        // zero vertical velocity then apply jump
        _playerRigidbody.linearVelocityY = 0f;
        _playerRigidbody.linearVelocityY = jumpPower;

        // ensure gravityScale is the default while starting the jump
        _playerRigidbody.gravityScale = _defaultGravityScale;
        CurrentlyJumping = true;

        PlayerAnimator.SetTrigger("Jump");

    }

    public void DoJump(float jumpPowerMultiplier)
    {
        //Debug.Log("Jumping from jump script");
        // Compute using the engine gravity and the default gravityScale so buffered jumps
        // don't inherit a high "falling" gravityScale and become overpowered.
        float gravity = Physics2D.gravity.y * _defaultGravityScale; // negative
        float jumpPower = Mathf.Sqrt(-2f * gravity * JumpHeight); // v = sqrt(2 * |g| * h)

        // zero vertical velocity then apply jump
        _playerRigidbody.linearVelocityY = 0f;
        _playerRigidbody.linearVelocityY = jumpPower * jumpPowerMultiplier;

        // ensure gravityScale is the default while starting the jump
        _playerRigidbody.gravityScale = _defaultGravityScale;
        CurrentlyJumping = true;

        PlayerAnimator.SetTrigger("Jump");

    }

    private void LimitFallSpeed()
    {
        if (_playerRigidbody.linearVelocityY < -SpeedLimit)
        {
            _playerRigidbody.linearVelocityY = -SpeedLimit;
        }
    }

    public bool GetJumping() { return CurrentlyJumping; }

    public bool GetAirJumping() { return _AirJumping; }

    public bool IsDescending() { return _playerRigidbody.linearVelocityY < 0; }

    public void SetAllowAirJumps(bool allowAirJumps) { AllowAirJumps = allowAirJumps; }

    public void SetMaxAirJumps(int maxAirJumps) { MaxAirJumps = maxAirJumps; }

    public void IncreaseMaxAirJumps()
    {
        MaxAirJumps++;
        OnMaxAirJumpsChanged?.Invoke(MaxAirJumps);
    }

    public void RenewAirJumps(int renewJumpAmount)
    {
        _airJumps += renewJumpAmount;
        OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);
    }
}
