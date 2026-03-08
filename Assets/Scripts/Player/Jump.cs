using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
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
    private float _jumpSpeed;
    public float TimeToJumpApex;

    public float AscendingGravity = 1f;
    public float DescendingGravity = 6.17f;
    private float _gravMultiplier;

    [Header("Optionals")]
    public bool AllowAirJump;
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

    void Update()
    {
        _onGround = _playerGround.GetOnGround();

        ResetAirJumps();

        CheckJumpPressed();

        HandleJumpBuffer();

        HandleCoyoteTime();

        //SetPhysics();
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
            jumpPressIndicator.GetComponent<Renderer>().material.color = Color.red;
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
        if (!_desireJump)
            return;

        // cleaned up redundant condition and keep intended behavior:
        // allow jump when on ground, during coyote time, or if air jumps remain
        if (_onGround || _coyoteTimer > 0f || _airJumps > 0)
        {
            CurrentlyJumping = true;
            if (!_onGround)
            {
                _airJumps--;
            }

            _desireJump = false;

            DoJump();
            _jumpBufferTimer = 0;
            _coyoteTimer = 0;
        }
    }

    private void ResetAirJumps()
    {
        if (_onGround && AllowAirJump)
        {
            _airJumps = MaxAirJumps;
        }
    }

    private void DoJump()
    {
        // Compute using the engine gravity and the default gravityScale so buffered jumps
        // don't inherit a high "falling" gravityScale and become overpowered.
        float gravity = Physics2D.gravity.y * _defaultGravityScale; // negative
        _jumpSpeed = Mathf.Sqrt(-2f * gravity * JumpHeight); // v = sqrt(2 * |g| * h)

        // zero vertical velocity then apply jump
        _playerRigidbody.linearVelocityY = 0f;
        _playerRigidbody.linearVelocityY = _jumpSpeed;

        // ensure gravityScale is the default while starting the jump
        _playerRigidbody.gravityScale = _defaultGravityScale;
        CurrentlyJumping = true;

        PlayerAnimator.SetTrigger("Jump");
    }
}
