using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset InputActions;
    private InputAction _move;

    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    public Grapple Grapple;
    private WallJump _wallJump;
    private Jump _jump;

    public Animator PlayerAnimator;

    [Header("Movement Settings")]
    public float MoveSpeed = 5f;
    private Vector2 _moveAmount;

    [Header("Ground Settings")]
    private bool _onGround = false;

    private bool _facingRight = true;

    private bool _moveButtonPressed;

    private void Awake()
    {
        // Use the assigned asset instead of InputSystem.actions
        _move = InputActions.FindAction("Move");
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
        _wallJump = GetComponent<WallJump>();
        _jump = GetComponent<Jump>();

    }

    private void Update()
    {
        _onGround = _playerGround.GetOnGround();

        _moveAmount = _move.ReadValue<Vector2>();
        Flip();

        PlayerAnimator.SetBool("Grounded", _onGround);

    }

    private void FixedUpdate()
    {
        if (Grapple.GetIsGrappling()) return;
        if (_wallJump.GetIsWallJumping()) return;
        Move();
    }

    private void Move()
    {


        _playerRigidbody.linearVelocity = new Vector2(_moveAmount.x * MoveSpeed, _playerRigidbody.linearVelocity.y);

        if (_moveAmount.x != 0)
        {
            _moveButtonPressed = true;
            PlayerAnimator.SetBool("Walking", true);
        }
        else
        {
            _moveButtonPressed = false;
            PlayerAnimator.SetBool("Walking", false);
        }
    }

    // Mirror the player's x-scale to flip sprite
    public void Flip()
    {
        if (_facingRight && _playerRigidbody.linearVelocityX < 0f || !_facingRight && _playerRigidbody.linearVelocityX > 0f)
        {
            _facingRight = !_facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    public void DisableMovementInput()
    {
        //Debug.Log("Disabling movement input");
        _move.Disable();
    }

    public void EnableMovementInput()
    {
        //Debug.Log("Enabling movement input");
        _move.Enable();
    }

    public bool GetMoveButtonPressed()
    {
        if(_moveAmount.x != 0)
        {
            _moveButtonPressed = true;
        }
        else
        {
            _moveButtonPressed = false;
        }
        return _moveButtonPressed;
    }

    public float GetMoveInputX()
    {
        return _moveAmount.x;
    }
}
