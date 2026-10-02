using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    public static event Action<bool> OnDash;

    [Header("Input References")]
    private HandlePlayerInput _handlePlayerInput;
    private InputActionAsset _inputActions;
    private InputAction _dashInput;

    [Header("Component References")]
    private Rigidbody2D _playerRigidbody;
    private Stamina _playerStamina;
    private PlayerGround _playerGround;
    private PlayerMovement _playerMovement;

    [Header("Dash Settings")]
    [SerializeField] private float _dashForce = 5f;
    [SerializeField] private float _dashDuration = 1f;
    private int _dashDirection = 1;
    [SerializeField] private float _dashForceMultiplier = 1f;
    private float _dashCooldown = 0.5f;
    private float _dashCooldownTimer = 0f;

    private float _defaultGravityScale;

    [Header("State")]
    private bool _canDash = true;
    private bool _desireDash = false;
    private bool _pressingDash = false;
    private bool _isDashing = false;
    private bool _onGround;


    private void Awake()
    {
        _handlePlayerInput = GetComponent<HandlePlayerInput>();

        _inputActions = _handlePlayerInput.InputActions;

        _dashInput = _inputActions.FindAction("Dash");

        _playerRigidbody = GetComponent<Rigidbody2D>();

        _playerGround = GetComponent<PlayerGround>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerStamina = GetComponent<Stamina>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        _onGround = _playerGround.GetOnGround();

        SetDashDirection();
        CheckDashPressed();
    }

    private void FixedUpdate()
    {
        if (_isDashing) return;

        if (_desireDash && _canDash && _playerStamina.GetStamina() > 0)
        {
            StartCoroutine(PerformDash());
        }
    }

    private void CheckDashPressed()
    {
        if (_dashInput != null && _dashInput.WasPressedThisFrame())
        {
            _desireDash = true;
            _pressingDash = true;
        }
        else if (_dashInput != null && _dashInput.WasReleasedThisFrame())
        {
            _desireDash = false;
            _pressingDash = false;
        }
    }

    private void SetDashDirection()
    {
        float moveInput = _playerMovement.GetMoveInputX();
        if (moveInput > 0)
        {
            _dashDirection = 1;
        }
        else if (moveInput < 0)
        {
            _dashDirection = -1;
        }
    }

    private IEnumerator PerformDash()
    {
        _playerStamina.UseStamina(1);

        OnDash?.Invoke(true);
        _canDash = false;
        _isDashing = true;
        _playerRigidbody.linearVelocity = Vector2.zero;

        float originalGravityScale = _playerRigidbody.gravityScale;
        _playerRigidbody.gravityScale = 0f;

        if (_dashDirection == 1) // Dash to the right
        {
            _playerRigidbody.linearVelocityX = 1 * _dashForce * _dashForceMultiplier;
        }
        else if (_dashDirection == -1) // Dash to the left
        {
            _playerRigidbody.linearVelocityX = -1 * _dashForce * _dashForceMultiplier;
        }
        else if (_dashDirection == 0) // Default dash direction (right) if no input is given
        {
            _playerRigidbody.linearVelocityX = 1 * _dashForce * _dashForceMultiplier;
        }
        yield return new WaitForSeconds(_dashDuration);
        _playerRigidbody.gravityScale = originalGravityScale;
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;

        //PlayerAnimator.SetTrigger("Dash");
    }

    private IEnumerator PauseGravity(float duration)
    {
        _playerRigidbody.gravityScale = 0f;

        yield return new WaitForSeconds(duration);

        _playerRigidbody.gravityScale = _defaultGravityScale;
        _isDashing = false;
        OnDash?.Invoke(false);
    }

    public bool GetIsDashing()
    {
        return _isDashing;
    }
}
