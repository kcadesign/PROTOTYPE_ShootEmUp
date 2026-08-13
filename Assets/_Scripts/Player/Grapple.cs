using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    [Header("References")]
    public PlayerGround PlayerGround;
    public Jump PlayerJump;
    public WallJump WallJump;
    public GameObject Player;
    public LineRenderer LineRenderer;

    private Rigidbody2D _playerRigidbody;
    private CircleCollider2D _playerGrappleCollider;

    private bool _isGrounded;
    private bool _canGrapple;
    private bool _isGrappling;

    private float _originalGravityScale;

    private Vector2 _grappleTarget;

    private List<Collider2D> _collidersList = new List<Collider2D>();

    private GameObject _closestGrapplePoint;

    [Header("Grapple Settings")]
    public GameObject GrappleTip;

    public float GrappleSpeed = 10f;
    public float LaunchForceMultiplier = 2f;

    // Tolerance in world units used to determine when
    // the player has reached the grapple point.
    public float GrappleTolerance = 0.05f;

    private void Awake()
    {
        _playerRigidbody = Player.GetComponent<Rigidbody2D>();
        _playerGrappleCollider = GetComponent<CircleCollider2D>();

        LineRenderer.enabled = false;
        GrappleTip.SetActive(false);
    }

    private void Update()
    {
        _isGrounded = PlayerGround.GetOnGround();

        UpdateClosestGrapplePoint();

        _canGrapple = CanGrapple();

        // Update the rope visually every rendered frame.
        // This does NOT move the player.
        if (_isGrappling)
        {
            LineRenderer.SetPosition(0, _playerRigidbody.position);

            LineRenderer.SetPosition(1, _grappleTarget);
        }
    }

    private void FixedUpdate()
    {
        if (!_isGrappling) return;

        UpdateGrappleMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (_playerGrappleCollider.IsTouching(collision))
        {
            if (collision.gameObject.CompareTag("GrapplePoint"))
            {
                if (!_collidersList.Contains(collision))
                {
                    _collidersList.Add(collision);
                }

                GrappleTip.SetActive(_collidersList.Count > 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("GrapplePoint"))
        {
            _collidersList.Remove(collision);

            if (_collidersList.Count == 0)
            {
                GrappleTip.SetActive(false);
            }
        }
    }

    private void UpdateClosestGrapplePoint()
    {
        if (_collidersList.Count == 0)
        {
            _closestGrapplePoint = null; return;
        }

        Vector2 playerPos = _playerRigidbody.position;

        Collider2D closest = null;
        float minSquare = float.MaxValue;

        for (int i = 0; i < _collidersList.Count; i++)
        {
            Collider2D collider = _collidersList[i];

            if (collider == null) continue;

            float square = ((Vector2)collider.transform.position - playerPos).sqrMagnitude;

            if (square < minSquare)
            {
                minSquare = square;
                closest = collider;
            }
        }

        _closestGrapplePoint = closest != null ? closest.gameObject : null;

        if (_closestGrapplePoint != null)
        {
            GrappleTip.transform.position = _closestGrapplePoint.transform.position;
        }
    }

    /// <summary>
    /// Returns true if the player can currently start a grapple.
    /// </summary>
    public bool CanGrapple()
    {
        if (_isGrounded)
            return false;

        if (_isGrappling)
            return false;

        if (_closestGrapplePoint == null)
            return false;

        if (WallJump != null && WallJump.GetOnWall())
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Starts the grapple state.
    ///
    /// IMPORTANT:
    /// This method does not move the player.
    /// All Rigidbody movement happens in FixedUpdate().
    /// </summary>
    public bool TryStartGrapple()
    {
        if (!CanGrapple())
            return false;

        // Capture the target when the grapple begins.
        // This prevents the target from changing while
        // the player is being pulled toward it.
        _grappleTarget = _closestGrapplePoint.transform.position;

        _originalGravityScale = _playerRigidbody.gravityScale;

        // Stop existing physics movement.
        _playerRigidbody.linearVelocity = Vector2.zero;

        // Disable gravity for the duration of the grapple.
        _playerRigidbody.gravityScale = 0f;

        _isGrappling = true;

        LineRenderer.enabled = true;

        LineRenderer.SetPosition(0, _playerRigidbody.position);

        LineRenderer.SetPosition(1, _grappleTarget);

        return true;
    }

    /// <summary>
    /// All grapple movement happens here.
    /// This is called exclusively from FixedUpdate().
    /// </summary>
    private void UpdateGrappleMovement()
    {
        Vector2 currentPosition = _playerRigidbody.position;

        float sqrDistance = (currentPosition - _grappleTarget).sqrMagnitude;

        float sqrTolerance = GrappleTolerance * GrappleTolerance;

        // We have reached the grapple point.
        if (sqrDistance <= sqrTolerance)
        {
            FinishGrapple();
            return;
        }

        Vector2 nextPosition = Vector2.MoveTowards(currentPosition, _grappleTarget, GrappleSpeed * Time.fixedDeltaTime);

        _playerRigidbody.MovePosition(nextPosition);
    }

    /// <summary>
    /// Finishes the grapple and launches the player.
    ///
    /// This is also called from FixedUpdate via
    /// UpdateGrappleMovement(), so the launch occurs
    /// within the physics loop.
    /// </summary>
    private void FinishGrapple()
    {
        // Make sure we land exactly on the target.
        _playerRigidbody.MovePosition(_grappleTarget);

        // Restore gravity.
        _playerRigidbody.gravityScale = _originalGravityScale;

        _isGrappling = false;

        LineRenderer.enabled = false;

        // Launch happens as part of the same
        // FixedUpdate physics cycle.
        LaunchPlayer();

        // Restore air jumps after grappling.
        PlayerJump.ResetAirJumps();
    }

    private void LaunchPlayer()
    {
        PlayerJump.DoJump(LaunchForceMultiplier);
    }

    public bool GetGrapplePointAvailable()
    {
        return _closestGrapplePoint != null;
    }

    public bool GetCanGrapple()
    {
        return _canGrapple;
    }

    public bool GetIsGrappling()
    {
        return _isGrappling;
    }

    public void IncreaseGrappleRange(float amount)
    {
        _playerGrappleCollider.radius += amount;
    }
}