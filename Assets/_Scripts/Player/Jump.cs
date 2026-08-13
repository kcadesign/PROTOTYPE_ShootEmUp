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

        RecalculateJumpPhysics();
    }

    private void OnEnable()
    {
        UIController.OnToggleAirJumpPressed += UIController_OnToggleAirJumpPressed;
    }

    private void OnDisable()
    {
        UIController.OnToggleAirJumpPressed -= UIController_OnToggleAirJumpPressed;
    }

    private void Start()
    {
        if (AllowAirJumps)
        {
            _airJumps = MaxAirJumps;

            OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);

            OnMaxAirJumpsChanged?.Invoke(MaxAirJumps);
        }

        if (_playerRigidbody != null)
        {
            _playerRigidbody.gravityScale = _defaultGravityScale;
        }
    }

    private void Update()
    {
        _onGround = _playerGround.GetOnGround();

        CheckDescending();

        if ((_onGround && AllowAirJumps) || Grapple.GetIsGrappling())
        {
            _isAirJumping = false;

            OnAirJump?.Invoke(false);

            ResetAirJumps();
        }

        CheckJumpPressed();

        HandleJumpBuffer();

        HandleCoyoteTime();

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

            // Testing indicator
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

        if (_jumpBufferTimer < 0f)
        {
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
        const float errorThreshold = 0.01f;

        if (_playerRigidbody.linearVelocityY > errorThreshold && _pressingJump)
        {
            _gravMultiplier = _defaultGravityScale * AscendingGravity;
        }
        else if (_playerRigidbody.linearVelocityY < -errorThreshold)
        {
            _gravMultiplier = _defaultGravityScale * DescendingGravity;
        }
        else
        {
            if (_onGround && Mathf.Abs(_playerRigidbody.linearVelocityY) <= errorThreshold)
            {
                _gravMultiplier = _defaultGravityScale;

                IsJumping = false;
            }
            else
            {
                _gravMultiplier = _defaultGravityScale * DescendingGravity;
            }
        }

        _playerRigidbody.gravityScale = _gravMultiplier;
    }

    private void CheckCanJump()
    {
        if (!_desireJump) return;

        // -------------------------------------------------
        // WALL JUMP
        // -------------------------------------------------

        if (_wallJump != null && (_wallJump.GetIsWallSLiding() || _wallJump.GetIsWallJumping()))
        {
            return;
        }

        // -------------------------------------------------
        // GRAPPLE PRIORITY
        // -------------------------------------------------
        //
        // If the player is airborne, Grapple gets first
        // opportunity to consume the jump input.
        //
        // This check happens in FixedUpdate().
        // TryStartGrapple() does NOT move the player.
        // Actual movement happens in Grapple.FixedUpdate().
        // -------------------------------------------------

        if (!_onGround)
        {
            bool grappleStarted = Grapple.TryStartGrapple();

            if (grappleStarted)
            {
                // The grapple consumed the jump input.

                _desireJump = false;

                _jumpBufferTimer = 0f;

                _coyoteTimer = 0f;

                return;
            }
        }

        // If a grapple has already started,
        // absolutely no jump should happen.
        if (Grapple.GetIsGrappling()) return;

        // -------------------------------------------------
        // NORMAL JUMP
        // -------------------------------------------------

        if (_onGround || _coyoteTimer > 0f || (AllowAirJumps && _airJumps > 0))
        {
            // Air jump
            if (!_onGround && AllowAirJumps && _coyoteTimer <= 0f)
            {
                HandleAirJump();

                return;
            }

            // Ground / coyote jump
            HandleStandardJump();
        }
    }

    private void HandleAirJump()
    {
        _isAirJumping = true;

        OnAirJump?.Invoke(true);

        _airJumps--;

        OnCurrentAirJumpAmountChanged?.Invoke(_airJumps
        );

        _desireJump = false;

        DoAirJump(AirJumpMultiplier);

        _jumpBufferTimer = 0;

        _coyoteTimer = 0;
    }

    private void HandleStandardJump()
    {
        IsJumping = true;

        _desireJump = false;

        DoJump();

        _jumpBufferTimer = 0;

        _coyoteTimer = 0;
    }

    public void DoJump()
    {
        float jumpPower = ComputeJumpVelocity();

        _playerRigidbody.linearVelocityY = 0f;

        _playerRigidbody.linearVelocityY = jumpPower;

        _playerRigidbody.gravityScale = _defaultGravityScale;

        IsJumping = true;

        PlayerAnimator.SetTrigger("Jump");
    }

    public void DoJump(float jumpPowerMultiplier)
    {
        float jumpPower = ComputeJumpVelocity();

        _playerRigidbody.linearVelocityY = 0f;

        _playerRigidbody.linearVelocityY = jumpPower * jumpPowerMultiplier;

        _playerRigidbody.gravityScale = _defaultGravityScale;

        IsJumping = true;

        PlayerAnimator.SetTrigger("Jump");
    }

    public void DoAirJump(float jumpPowerMultiplier)
    {
        float jumpPower = ComputeJumpVelocity();

        OnAirJump?.Invoke(true);

        _playerRigidbody.linearVelocityY = 0f;

        _playerRigidbody.linearVelocityY = jumpPower * jumpPowerMultiplier;

        _playerRigidbody.gravityScale = _defaultGravityScale;

        _isAirJumping = true;

        PlayerAnimator.SetTrigger("Jump");
    }

    private float ComputeJumpVelocity()
    {
        float t = Mathf.Max(0.0001f, TimeToJumpApex);

        return (2f * JumpHeight) / t;
    }

    private void RecalculateJumpPhysics()
    {
        if (TimeToJumpApex <= 0f)
        {
            TimeToJumpApex = 0.1f;
        }

        float desiredGravityY = (-2f * JumpHeight) / (TimeToJumpApex * TimeToJumpApex);

        _defaultGravityScale = desiredGravityY / Physics2D.gravity.y;

        if (float.IsNaN(_defaultGravityScale) || _defaultGravityScale <= 0f)
        {
            _defaultGravityScale = 1f;
        }
    }

    private void UIController_OnToggleAirJumpPressed()
    {
        AllowAirJumps = !AllowAirJumps;

        if (AllowAirJumps)
        {
            MaxAirJumps = 1;

            _airJumps = MaxAirJumps;
        }
        else
        {
            MaxAirJumps = 0;

            _airJumps = 0;
        }

        OnMaxAirJumpsChanged?.Invoke(MaxAirJumps);

        OnCurrentAirJumpAmountChanged?.Invoke(_airJumps);
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

    public bool GetIsJumping()
    {
        return IsJumping;
    }

    public bool GetIsAirJumping()
    {
        return _isAirJumping;
    }

    public bool IsDescending()
    {
        return _playerRigidbody.linearVelocityY < 0;
    }

    public void SetAllowAirJumps(bool allowAirJumps)
    {
        AllowAirJumps = allowAirJumps;
    }

    public void SetMaxAirJumps(int maxAirJumps)
    {
        MaxAirJumps = maxAirJumps;
    }

    public void IncreaseMaxAirJumps()
    {
        MaxAirJumps++;

        OnMaxAirJumpsChanged?.Invoke(MaxAirJumps);
    }

    public void RenewAirJumps(int renewJumpAmount)
    {
        if (_airJumps == MaxAirJumps) return;

        _airJumps += renewJumpAmount;

        OnCurrentAirJumpAmountChanged?.Invoke(
            _airJumps
        );
    }

    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (TimeToJumpApex <= 0f)
            {
                TimeToJumpApex = 0.1f;
            }

            if (JumpHeight <= 0f)
            {
                JumpHeight = Mathf.Max(0.1f, JumpHeight);
            }
        }

        RecalculateJumpPhysics();
    }
}