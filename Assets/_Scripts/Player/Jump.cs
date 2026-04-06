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
    public float JumpHeight = 7.3f;
    public float TimeToJumpApex;
    public float AirJumpMultiplier = 1.1f;

    public float AscendingGravity = 1f;
    public float DescendingGravity = 6.17f;
    private float _gravMultiplier;

    [Header("Optionals")]
    public bool AllowAirJumps;
    private int _airJumps = 0;
    public int MaxAirJumps = 1;

    [Header("Buffers")]
    public float CoyoteTime = 0.15f;
    private float _coyoteTimer = 0;
    public float JumpBuffer = 0.15f;
    private float _jumpBufferTimer;

    [Header("Defaults & Limits")]
    // this now holds the base gravityScale required to reach JumpHeight in TimeToJumpApex
    private float _defaultGravityScale;
    public float SpeedLimit;

    [Header("Current State")]
    private bool _pressingJump;
    private bool _desireJump;
    public bool IsJumping;
    private bool _isAirJumping;
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

        // compute the gravityScale needed to achieve JumpHeight in TimeToJumpApex
        RecalculateJumpPhysics();
    }

    private void Start()
    {
        if (AllowAirJumps)
        {
            _airJumps = MaxAirJumps;
            OnCurrentAirJumpAmountChanged?.Invoke(_airJumps); // Notify initial health
            OnMaxAirJumpsChanged?.Invoke(MaxAirJumps); // Notify initial max health
        }

        // make sure the rigidbody uses the base gravity scale when starting
        if (_playerRigidbody != null)
            _playerRigidbody.gravityScale = _defaultGravityScale;
    }

    void Update()
    {
        _onGround = _playerGround.GetOnGround();

        CheckDescending();

        if (_onGround && AllowAirJumps || Grapple.GetIsGrappling())
        {
            _isAirJumping = false;
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

    //private void SetPhysics()
    //{
    //    Vector2 newGravity = new Vector2(0, (-2 * JumpHeight) / (TimeToJumpApex * TimeToJumpApex));
    //    _playerRigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * _gravMultiplier;
    //}


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
        if (!IsJumping && !_onGround)
        {
            _coyoteTimer -= Time.deltaTime;
        }

    }

    private void CalculateGravity()
    {
        // small threshold to ignore tiny floating-point fluctuations
        const float errorThreshold = 0.01f;

        // use AscendingGravity/DescendingGravity as multipliers of the base gravity scale
        if (_playerRigidbody.linearVelocityY > errorThreshold && _pressingJump)
        {
            // moving up
            _gravMultiplier = _defaultGravityScale * AscendingGravity;
        }
        else if (_playerRigidbody.linearVelocityY < -errorThreshold)
        {
            // falling
            _gravMultiplier = _defaultGravityScale * DescendingGravity;
        }
        else
        {
            // near zero vertical speed
            if (_onGround && Mathf.Abs(_playerRigidbody.linearVelocityY) <= errorThreshold)
            {
                // on ground and not moving -> base gravity and end jumping state
                _gravMultiplier = _defaultGravityScale;
                IsJumping = false;
            }
            else
            {
                // at apex in mid-air -> start falling
                _gravMultiplier = _defaultGravityScale * DescendingGravity;
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
            IsJumping = true;
            if (!_onGround && AllowAirJumps && !Grapple.GetIsGrappling())
            {
                _isAirJumping = true;
                OnAirJump?.Invoke(true);
                _airJumps--;
                OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);
                _desireJump = false;

                DoAirJump(AirJumpMultiplier);
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

    public void DoJump()
    {
        //Debug.Log("Jumping from jump script");
        // Compute using the desired time to apex so jumps reach JumpHeight at TimeToJumpApex
        float jumpPower = ComputeJumpVelocity();

        // zero vertical velocity then apply jump
        _playerRigidbody.linearVelocityY = 0f;
        _playerRigidbody.linearVelocityY = jumpPower;

        // ensure gravityScale is the default while starting the jump
        _playerRigidbody.gravityScale = _defaultGravityScale;
        IsJumping = true;

        PlayerAnimator.SetTrigger("Jump");

    }

    public void DoJump(float jumpPowerMultiplier)
    {
        //Debug.Log("Jumping from jump script");
        float jumpPower = ComputeJumpVelocity();

        // zero vertical velocity then apply jump
        _playerRigidbody.linearVelocityY = 0f;
        _playerRigidbody.linearVelocityY = jumpPower * jumpPowerMultiplier;

        // ensure gravityScale is the default while starting the jump
        _playerRigidbody.gravityScale = _defaultGravityScale;
        IsJumping = true;

        PlayerAnimator.SetTrigger("Jump");

    }

    public void DoAirJump(float jumpPowerMultiplier)
    {
        // Compute using the desired time to apex so jumps reach JumpHeight at TimeToJumpApex
        float jumpPower = ComputeJumpVelocity();

        OnAirJump?.Invoke(true);

        // zero vertical velocity then apply jump
        _playerRigidbody.linearVelocityY = 0f;
        _playerRigidbody.linearVelocityY = jumpPower * jumpPowerMultiplier;

        // ensure gravityScale is the default while starting the jump
        _playerRigidbody.gravityScale = _defaultGravityScale;
        _isAirJumping = true;

        PlayerAnimator.SetTrigger("Jump");

    }

    // compute v0 = 2 * h / t (stable, avoids depending on temporary gravityScale)
    private float ComputeJumpVelocity()
    {
        // guard against invalid TimeToJumpApex
        float t = Mathf.Max(0.0001f, TimeToJumpApex);
        return (2f * JumpHeight) / t;
    }

    // recalculates the base gravity scale so that an initial upward velocity of ComputeJumpVelocity()
    // will reach JumpHeight at TimeToJumpApex.
    private void RecalculateJumpPhysics()
    {
        // guard
        if (TimeToJumpApex <= 0f) TimeToJumpApex = 0.1f;

        // desired engine gravity (negative value)
        float desiredGravityY = (-2f * JumpHeight) / (TimeToJumpApex * TimeToJumpApex);

        // convert to gravityScale relative to Physics2D.gravity.y
        // Physics2D.gravity.y is negative in Unity by default, so this yields a positive scale.
        _defaultGravityScale = desiredGravityY / Physics2D.gravity.y;

        // ensure we have a sensible minimum gravity scale
        if (float.IsNaN(_defaultGravityScale) || _defaultGravityScale <= 0f)
            _defaultGravityScale = 1f;
    }

    public void ResetAirJumps()
    {
        _airJumps = MaxAirJumps;
        OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);
    }

    private void LimitFallSpeed()
    {
        if (_playerRigidbody.linearVelocityY < -SpeedLimit)
        {
            _playerRigidbody.linearVelocityY = -SpeedLimit;
        }
    }

    public bool GetIsJumping() { return IsJumping; }

    public bool GetIsAirJumping() { return _isAirJumping; }

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

    // Called in the editor when values are changed so the gravity scale updates immediately.
    private void OnValidate()
    {
        // Avoid running in edit mode before Rigidbody exists
        if (!Application.isPlaying)
        {
            // Attempt to compute sensible defaults; do not access instance fields that may be null.
            if (TimeToJumpApex <= 0f) TimeToJumpApex = 0.1f;
            if (JumpHeight <= 0f) JumpHeight = Mathf.Max(0.1f, JumpHeight);
        }

        RecalculateJumpPhysics();
    }
}
