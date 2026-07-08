using UnityEngine;

public class Walk : MonoBehaviour
{
    [Header("Movement Stats")]
    //public float StartDelay = 0;
    public float WalkSpeed = 4f;

    [Header("Ground Check Rays")]
    public float RayLength = 0.5f;
    public float RayOffset = 0.5f;
    public float RayGap = 0.5f;

    private bool _wallLeft = false;
    private bool _wallRight = false;
    private bool _groundLeft = false;
    private bool _groundRight = false;

    private bool _moveLeft = false;

    [SerializeField] private LayerMask _wallCheckLayers;
    [SerializeField] private LayerMask _groundLayer;


    private void Update()
    {

        RaycastHit2D wallLeft = Physics2D.Raycast(transform.position + Vector3.left * RayOffset, Vector2.left, RayLength, _wallCheckLayers);
        RaycastHit2D wallRight = Physics2D.Raycast(transform.position + Vector3.right * RayOffset, Vector2.right, RayLength, _wallCheckLayers);
        _wallLeft = wallLeft.collider != null;
        _wallRight = wallRight.collider != null;

        // create 2 downwards raycasts and check both for ground detection, this is to prevent the player from being detected as in the air when only one of the raycasts hits the ground
        RaycastHit2D groundLeft = Physics2D.Raycast(transform.position + Vector3.left * RayGap, Vector2.down, RayLength, _groundLayer);
        RaycastHit2D groundRight = Physics2D.Raycast(transform.position + Vector3.right * RayGap, Vector2.down, RayLength, _groundLayer);
        _groundLeft = groundLeft.collider != null;
        _groundRight = groundRight.collider != null;


        if (!_groundRight || _wallRight)
        {
            _moveLeft = true;
        }
        else if (!_groundLeft || _wallLeft)
        {
            _moveLeft = false;
        }


        //_jumpTimer -= Time.deltaTime;
        //if (_jumpTimer <= 0)
        //{
        //    Jump();
        //    _jumpTimer = JumpRate;
        //}
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_moveLeft)
        {
            transform.Translate(Vector2.left * WalkSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector2.right * WalkSpeed * Time.deltaTime);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = _wallLeft ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * RayOffset, transform.position + Vector3.left * RayOffset + Vector3.left * RayLength);

        // Draw right ray with its own color based on _wallRight
        Gizmos.color = _wallRight ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right * RayOffset, transform.position + Vector3.right * RayOffset + Vector3.right * RayLength);

        if (_groundLeft) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + Vector3.left * RayGap, transform.position + Vector3.left * RayGap + Vector3.down * RayLength);

        if (_groundRight) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + Vector3.right * RayGap, transform.position + Vector3.right * RayGap + Vector3.down * RayLength);
    }


}
