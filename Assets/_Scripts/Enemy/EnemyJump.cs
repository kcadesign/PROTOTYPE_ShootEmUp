using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    private Rigidbody2D _enemyRigidbody;

    private bool _wallLeft = false;
    private bool _wallRight = false;

    private bool _moveLeft = false;

    [Header("Jump Stats")]
    public float StartDelay = 0;
    public float JumpRate = 4f;
    public Vector2 JumpDirectionPower = new Vector2(1, 1);
    public float JumpPowerMultiplier = 1f;

    //private float _randomisedJumpDelay = 0;
    private float _jumpTimer = 0;
    [Header("Wall Check Rays")]
    public float RayLength = 0.5f;
    public float RayOffset = 0.5f;
    [SerializeField] private LayerMask _wallLayer;


    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        //_randomisedJumpDelay = Random.Range(JumpDelay + 1, JumpDelay - 1);
        //StartDelay = Random.Range(0, 3);

        // initialize movement direction from Direction.x so the enemy keeps moving until a wall is hit
    }

    private void Start()
    {
        _moveLeft = JumpDirectionPower.x < 0;
        _jumpTimer = StartDelay;
    }

    private void Update()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + Vector3.left * RayOffset, Vector2.left, RayLength, _wallLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + Vector3.right * RayOffset, Vector2.right, RayLength, _wallLayer);
        _wallLeft = hitLeft.collider != null;
        _wallRight = hitRight.collider != null;

        // only change direction when a wall is detected
        if (_wallRight)
        {
            _moveLeft = true;
        }
        else if (_wallLeft)
        {
            _moveLeft = false;
        }

        _jumpTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (_jumpTimer <= 0)
        {
            Jump();
            _jumpTimer = JumpRate;
        }

    }


    private void Jump()
    {
        if (_moveLeft)
        {
            _enemyRigidbody.linearVelocity = new Vector2(-JumpDirectionPower.x, JumpDirectionPower.y) * JumpPowerMultiplier;
        }
        else
        {
            _enemyRigidbody.linearVelocity = new Vector2(JumpDirectionPower.x, JumpDirectionPower.y) * JumpPowerMultiplier;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw left ray with its own color based on _wallLeft
        Gizmos.color = _wallLeft ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position + Vector3.left * RayOffset, transform.position + Vector3.left * RayOffset + Vector3.left * RayLength);

        // Draw right ray with its own color based on _wallRight
        Gizmos.color = _wallRight ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position + Vector3.right * RayOffset, transform.position + Vector3.right * RayOffset + Vector3.right * RayLength);
    }

}
