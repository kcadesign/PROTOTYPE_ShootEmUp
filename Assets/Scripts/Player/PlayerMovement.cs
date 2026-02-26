using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction _move;

    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    public Grapple Grapple;

    public Animator PlayerAnimator;

    [Header("Movement Settings")]
    private Vector2 _moveAmount;
    public float MoveSpeed = 5f;

    [Header("Ground Settings")]
    private bool _onGround = false;

    private bool _facingRight = true;

    private void Awake()
    {
        // Use the assigned asset instead of InputSystem.actions
        _move = InputActions.FindAction("Move");
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerGround = GetComponent<PlayerGround>();
    }

    private void Update()
    {
        _onGround = _playerGround.GetOnGround();

        _moveAmount = _move.ReadValue<Vector2>();

        PlayerAnimator.SetBool("Grounded", _onGround);

        if (Grapple.IsGrappling()) return;
        Move();

    }


    private void Move()
    {
        Vector2 movement = new Vector2(_moveAmount.x * MoveSpeed, _playerRigidbody.linearVelocity.y);

        // Flip sprite based on horizontal input direction
        if (movement.x > 0f && !_facingRight)
        {
            Flip();
        }
        else if (movement.x < 0f && _facingRight)
        {
            Flip();
        }

        if (movement.x != 0)
        {
            _playerRigidbody.linearVelocity = movement;
            PlayerAnimator.SetBool("Walking", true);
        }
        else
        {
            PlayerAnimator.SetBool("Walking", false);
        }

    }

    // Mirror the player's x-scale to flip sprite
    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

}
