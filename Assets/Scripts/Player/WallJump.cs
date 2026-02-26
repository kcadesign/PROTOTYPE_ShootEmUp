using UnityEngine;
using UnityEngine.InputSystem;

public class WallJump : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset InputActions;
    private InputAction _jump;

    private Rigidbody2D _playerRigidbody;
    private PlayerGround _playerGround;


    [Header("Ray Settings")]
    [SerializeField][Tooltip("Length of the ground-checking collider")] private float _rayLength = 0.95f;
    [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 _rayOffset;

    [Header("Layer Masks")]
    [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask _wallLayer;

    private bool _onWallRight;
    private bool _onWallLeft;

    [Header("Wall Jump Values")]
    public float GravityScaleOnWall = 0.1f;
    public float WallJumpForceX = 10f;
    public float WallJumpForceY = 10f;
    private bool _canWallJump;

    private void Awake()
    {
        _jump = InputActions.FindAction("Jump");
        _playerGround = GetComponent<PlayerGround>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Debug.Log("Can wall jump: " + _canWallJump);

        // fire a ray to the right and left of the player to check for walls
        _onWallRight = Physics2D.Raycast(transform.position + _rayOffset, Vector2.right, _rayLength, _wallLayer);
        _onWallLeft = Physics2D.Raycast(transform.position - _rayOffset, Vector2.left, _rayLength, _wallLayer);

        if (!_onWallRight && !_onWallLeft || _playerGround.GetOnGround())
        {
            _canWallJump = true;
        }

        if (_jump.WasPressedThisFrame() && _onWallRight && !_playerGround.GetOnGround() && _canWallJump)
        {
            //Debug.Log("Can wall jump: " + _canWallJump);
            _canWallJump = false;

            //_playerRigidbody.linearVelocity = Vector2.zero; // Reset the player's velocity before applying the bounce
            _playerRigidbody.linearVelocity = new Vector2(-WallJumpForceX, WallJumpForceY);
            //_playerRigidbody.AddForce(Vector2.left * WallJumpForce, ForceMode2D.Impulse);
        }
        else if (_jump.WasPressedThisFrame() && _onWallLeft && !_playerGround.GetOnGround() && _canWallJump)
        {
            //Debug.Log("Can wall jump: " + _canWallJump);
            _canWallJump = false;

            //_playerRigidbody.linearVelocity = Vector2.zero; // Reset the player's velocity before applying the bounce
            _playerRigidbody.linearVelocity = new Vector2(WallJumpForceX, WallJumpForceY);
            //_playerRigidbody.AddForce(Vector2.right * WallJumpForce, ForceMode2D.Impulse);
        }

    }

    private void OnDrawGizmos()
    {
        //Draw the wall rays on screen for debug purposes
        if (_onWallRight || _onWallLeft) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + _rayOffset, transform.position + _rayOffset + Vector3.right * _rayLength);
        Gizmos.DrawLine(transform.position - _rayOffset, transform.position - _rayOffset + Vector3.left * _rayLength);
    }

    //Send wall detection to other scripts
    public bool GetOnWall()
    {
        bool onWall = _onWallRight || _onWallLeft;
        return onWall;
    }
}
