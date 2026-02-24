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

        PlayerAnimator.SetBool("Grounded", _onGround );
    }

    private void FixedUpdate()
    {
        if(Grapple.IsGrappling()) return;
        Move();
    }

    private void Move()
    {
        Vector2 movement = new Vector2(_moveAmount.x * MoveSpeed, _playerRigidbody.linearVelocity.y);
        _playerRigidbody.linearVelocity = movement;

        if (movement.x != 0)
        {
            // play walking animation
            PlayerAnimator.SetBool("Walking", true);
        }
        else
        {
            // stop walking animation
            PlayerAnimator.SetBool("Walking", false);
        }
    }

}
