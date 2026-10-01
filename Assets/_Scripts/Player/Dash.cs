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
    private InputAction _dash;

    [Header("Component References")]
    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    private PlayerMovement _playerMovement;

    [Header("Dash Settings")]
    [SerializeField] private float _dashForce = 5f;
    [SerializeField] private float _dashDuration = 0.1f;
    private int _dashDirection = 1;
    [SerializeField] private float _dashForceMultiplier = 1f;

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

        _dash = _inputActions.FindAction("Dash");

        _playerRigidbody = GetComponent<Rigidbody2D>();

        _playerGround = GetComponent<PlayerGround>();
        _playerMovement = GetComponent<PlayerMovement>();
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
        if (_desireDash)
        {
            PerformDash();
        }
    }

    private void CheckDashPressed()
    {
        if (_dash != null && _dash.WasPressedThisFrame())
        {
            _desireDash = true;
            _pressingDash = true;
        }
        else if (_dash != null && _dash.WasReleasedThisFrame())
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

    private void PerformDash()
    {
        OnDash?.Invoke(true);

        _playerRigidbody.linearVelocity = Vector2.zero;


        if (_dashDirection == 1)
        {
            _playerRigidbody.AddForce(Vector2.right * _dashForce * _dashForceMultiplier, ForceMode2D.Impulse);
        }
        else if (_dashDirection == -1)
        {
            _playerRigidbody.AddForce(Vector2.left * _dashForce * _dashForceMultiplier, ForceMode2D.Impulse);
        }
        else if (_dashDirection == 0)
        {
            _playerRigidbody.AddForce(Vector2.right * _dashForce * _dashForceMultiplier, ForceMode2D.Impulse);
        }

        _isDashing = true;

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
}
