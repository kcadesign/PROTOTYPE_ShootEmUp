using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset InputActions;
    private InputAction _jump;
    public PlayerGround PlayerGround;
    public Jump PlayerJump;
    public WallJump WallJump;
    public GameObject Player;
    private Rigidbody2D _playerRigidbody;
    public LineRenderer LineRenderer;
    private CircleCollider2D _playerGrappleCollider;

    private bool _isGrounded;
    private bool _isGrappling;

    private List<Collider2D> _collidersList = new List<Collider2D>();

    private GameObject _closestGrapplePoint;

    [Header("Grapple Settings")]
    public GameObject GrappleTip;

    public float GrappleSpeed = 10f;
    public float LaunchForceMultiplier = 2f;
    // tolerance (in world units) used to avoid exact float equality when checking arrival
    public float GrappleTolerance = 0.05f;

    private void Awake()
    {
        _jump = InputActions.FindAction("Jump");
        _playerRigidbody = Player.GetComponent<Rigidbody2D>();
        _playerGrappleCollider = GetComponent<CircleCollider2D>();

        LineRenderer.enabled = false;
        GrappleTip.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (_playerGrappleCollider.IsTouching(collision))
        {
            if (collision.gameObject.CompareTag("GrapplePoint"))
            {
                //Debug.Log($"{collision.gameObject.name} - entered grapple range");
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

            // hide tip if no candidates remain
            if (_collidersList.Count == 0)
            {
                GrappleTip.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = PlayerGround.GetOnGround();
        if (_jump.WasPressedThisFrame()
            && !_isGrounded
            && _closestGrapplePoint != null
            && !WallJump.GetOnWall())
        {
            StartCoroutine(GrappleCoroutine());
            //_isGrappling = true;
        }
    }

    private void Update()
    {
        UpdateClosestGrapplePoint();
    }

    private void UpdateClosestGrapplePoint()
    {
        if (_collidersList.Count == 0)
        {
            _closestGrapplePoint = null;
            return;
        }

        Vector2 playerPos = (Vector2)transform.position;
        Collider2D closest = null;
        float minSquare = float.MaxValue;

        // Compare squared distances to avoid square roots
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

        // Optionally update GrappleTip position immediately when closest changes
        if (_closestGrapplePoint != null)
        {
            GrappleTip.transform.position = _closestGrapplePoint.transform.position;
        }
    }

    private IEnumerator GrappleCoroutine()
    {
        Vector3 desiredGrapplePosition = _closestGrapplePoint.transform.position;
        float originalGravityScale = _playerRigidbody.gravityScale;
        float sqrTolerance = GrappleTolerance * GrappleTolerance;

        // prepare physics state for controlled movement
        _playerRigidbody.linearVelocity = Vector2.zero;
        _playerRigidbody.gravityScale = 0f;

        _isGrappling = true;
        LineRenderer.enabled = true;

        // move using FixedUpdate / physics to keep consistent behaviour across frame rates
        while (((Vector2)Player.transform.position - (Vector2)desiredGrapplePosition).sqrMagnitude > sqrTolerance)
        {
            LineRenderer.SetPosition(0, Player.transform.position);
            LineRenderer.SetPosition(1, desiredGrapplePosition);

            Vector2 currentPos = _playerRigidbody.position;
            Vector2 nextPos = Vector2.MoveTowards(currentPos, desiredGrapplePosition, GrappleSpeed * Time.fixedDeltaTime);
            _playerRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        // snap to exact target to avoid tiny residual differences
        _playerRigidbody.MovePosition(desiredGrapplePosition);
        Player.transform.position = desiredGrapplePosition;

        Debug.Log("Reached grapple point");
        LineRenderer.enabled = false;
        _playerRigidbody.gravityScale = originalGravityScale; // re-enable gravity after grappling
        _isGrappling = false;
        LaunchPlayer();
        Debug.Log("Launched player");

        PlayerJump.ResetAirJumps();

    }

    private void LaunchPlayer()
    {
        PlayerJump.DoJump(LaunchForceMultiplier);
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
