using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grapple : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset InputActions;
    private InputAction _jump;
    public PlayerGround _playerGround;
    public PlayerJump _playerJump;
    public WallJump _wallJump;
    public GameObject Player;
    private Rigidbody2D _playerRigidbody;
    public LineRenderer _lineRenderer;

    private bool _isGrounded;
    private bool _isGrappling;


    private List<Collider2D> _collidersList = new List<Collider2D>();

    private GameObject _closestGrapplePoint;

    [Header("Grapple Settings")]
    public GameObject GrappleTip;

    public float GrappleSpeed = 10f;
    public float LaunchForce = 10f;

    private void Awake()
    {
        _jump = InputActions.FindAction("Jump");
        _playerRigidbody = Player.GetComponent<Rigidbody2D>();

        _lineRenderer.enabled = false;
        GrappleTip.SetActive(false);
    }

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.CompareTag("GrapplePoint"))
        {
            if (!_collidersList.Contains(collision))
            {
                _collidersList.Add(collision);
            }

            GrappleTip.SetActive(_collidersList.Count > 0);
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

    private void Update()
    {
        UpdateClosestGrapplePoint();

        _isGrounded = _playerGround.GetOnGround();
        if (_jump.WasPressedThisFrame() 
            && !_isGrounded 
            && _closestGrapplePoint != null 
            && !_wallJump.GetOnWall())
        {
            StartCoroutine(GrappleCoroutine());
            _isGrappling = true;
        }
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
        while (Player.transform.position != desiredGrapplePosition)
        {
            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, desiredGrapplePosition);
            _playerRigidbody.linearVelocity = Vector2.zero; // reset velocity to prevent physics interference
            _playerRigidbody.gravityScale = 0f; // disable gravity while grappling
            Player.transform.position = Vector3.MoveTowards(transform.position, desiredGrapplePosition, GrappleSpeed * Time.deltaTime);
            yield return null;
        }
        _lineRenderer.enabled = false;
        _playerRigidbody.gravityScale = originalGravityScale; // re-enable gravity after grappling
        LaunchPlayer();
        _isGrappling = false;
        _playerJump.ResetAirJump();

    }

    private void LaunchPlayer()
    {
        _playerRigidbody.AddForce(new Vector2(_playerRigidbody.linearVelocityX, 1 * LaunchForce), ForceMode2D.Impulse);
    }

    public bool IsGrappling()
    {
        return _isGrappling;
    }
}
