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

    private bool _groundLeft = false;
    private bool _groundRight = false;

    private bool _moveLeft = false;

    [SerializeField] private LayerMask _groundLayer;


    private void Update()
    {

        //RaycastHit2D hit = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, _groundLayer);
        //_onGround = hit.collider != null;

        // create 2 downwards raycasts and check both for ground detection, this is to prevent the player from being detected as in the air when only one of the raycasts hits the ground
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + Vector3.left * RayGap, Vector2.down, RayLength, _groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + Vector3.right * RayGap, Vector2.down, RayLength, _groundLayer);
        _groundLeft = hitLeft.collider != null;
        _groundRight = hitRight.collider != null;

        // only change direction when a wall is detected
        if (!_groundRight)
        {
            _moveLeft = true;
        }
        else if (!_groundLeft)
        {
            _moveLeft = false;
        }

        Move();

        //_jumpTimer -= Time.deltaTime;
        //if (_jumpTimer <= 0)
        //{
        //    Jump();
        //    _jumpTimer = JumpRate;
        //}
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
        if (_groundLeft) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        //Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + Vector3.left * RayGap, transform.position + Vector3.left * RayGap + Vector3.down * RayLength);

        if (_groundRight) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + Vector3.right * RayGap, transform.position + Vector3.right * RayGap + Vector3.down * RayLength);
    }


}
