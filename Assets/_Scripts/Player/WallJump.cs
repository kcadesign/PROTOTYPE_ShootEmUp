using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallJump : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset InputActions;
    private InputAction _jump;

    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;
    private PlayerMovement _playerMove;

    [Header("Ray Settings")]
    [SerializeField] private float _rayLength = 0.95f;
    [SerializeField] private Vector3 _rayOffset;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _wallLayer;

    private bool _onWallRight;
    private bool _onWallLeft;

    [Header("Wall Jump Values")]
    public float _wallSlideSpeed;
    private bool _isWallSliding;
    private bool _isWallJumping;
    private float _wallJumpDirection;
    private float _wallJumpTime = 0.2f;
    private float _wallJumpCounter;
    public float WallJumpDuration = 0.5f;
    public Vector2 WallJumpPower;
    //public float GravityScaleOnWall = 0.1f;
    //public float WallJumpForceX = 10f;
    //public float WallJumpForceY = 10f;
    //private bool _canWallJump;
    //public float DisableMoveInputDuration = 0.2f;

    private void Awake()
    {
        _jump = InputActions.FindAction("Jump");
        _playerGround = GetComponent<PlayerGround>();
        _playerMove = GetComponent<PlayerMovement>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Debug.Log("Can wall jump: " + _canWallJump);

        // fire a ray to the right and left of the player to check for walls
        _onWallRight = Physics2D.Raycast(transform.position + _rayOffset, Vector2.right, _rayLength, _wallLayer);
        _onWallLeft = Physics2D.Raycast(transform.position - _rayOffset, Vector2.left, _rayLength, _wallLayer);

        WallSlide();
        DoWallJump();

    }

    private void OnDrawGizmos()
    {
        //Draw the wall rays on screen for debug purposes
        if (_onWallRight || _onWallLeft) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + _rayOffset, transform.position + _rayOffset + Vector3.right * _rayLength);
        Gizmos.DrawLine(transform.position - _rayOffset, transform.position - _rayOffset + Vector3.left * _rayLength);
    }

    //Send wall detection to other scripts

    private void WallSlide()
    {
        if(GetOnWall() && !_playerGround.GetOnGround() && _playerMove.GetMoveButtonPressed())
        {
            _isWallSliding = true;
            _playerRigidbody.linearVelocity = new Vector2(_playerRigidbody.linearVelocityX, Mathf.Clamp(_playerRigidbody.linearVelocityY,-_wallSlideSpeed, _playerRigidbody.linearVelocityY)) ;
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void DoWallJump()
    {
        if (_isWallSliding)
        {
            _isWallJumping = false;
            _wallJumpDirection = -transform.localScale.x;
            _wallJumpCounter = _wallJumpTime;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            _wallJumpCounter -= Time.deltaTime;
        }

        if (_jump.WasPressedThisFrame() && _wallJumpCounter > 0)
        {
            _isWallJumping = true;
            _playerMove.DisableMovementInput();
            // if the player is pressing in the direction of the wall jump then add extra power to the jump
            _playerRigidbody.linearVelocity = new Vector2(WallJumpPower.x * _wallJumpDirection, WallJumpPower.y);
            _wallJumpCounter = 0;

            if (transform.localScale.x == _wallJumpDirection)
            {
                _playerMove.Flip();
            }
            Invoke(nameof(StopWallJumping), WallJumpDuration);
        }
    }

    public bool GetOnWall()
    {
        bool onWall = _onWallRight || _onWallLeft;
        return onWall;
    }

    private void StopWallJumping()
    {
        _isWallJumping = false;
        _playerMove.EnableMovementInput();
    }

    public bool GetIsWallJumping()
    {
        return _isWallJumping;
    }

    private IEnumerator DisableMoveInput(float duration)
    {
        _playerMove.DisableMovementInput();
        yield return new WaitForSeconds(duration);
        _playerMove.EnableMovementInput();
    }
}
