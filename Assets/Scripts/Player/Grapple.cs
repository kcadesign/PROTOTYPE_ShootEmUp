using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.WSA;

public class Grapple : MonoBehaviour
{
    [Header("References")]
    public InputActionAsset InputActions;
    private InputAction _jump;
    public PlayerGround _playerGround;
    private bool _isGrounded;
    private bool _isGrappling;


    [SerializeField] private Rigidbody2D _rigidbody;
    public GameObject Player;
    public LineRenderer _lineRenderer;
    public DistanceJoint2D _distanceJoint;

    private List<Collider2D> _collidersList = new List<Collider2D>();

    private GameObject _closestGrapplePoint;

    public LayerMask GrappleLayerMask;

    public GameObject GrappleTip;

    public float GrappleTime = 10f;
    public float LaunchForce = 10f;

    private void Awake()
    {
        _jump = InputActions.FindAction("Jump");

        _lineRenderer.enabled = false;
        _distanceJoint.enabled = false;
        GrappleTip.SetActive(false);
    }

    private void OnEnable()
    {
        if (!InputActions.FindActionMap("Player").enabled)
        {
            InputActions.FindActionMap("Player").Enable();
        }
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
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

            //_lineRenderer.enabled = true;
            //_distanceJoint.enabled = true;

            //_distanceJoint.connectedBody = _closestGrapplePoint.GetComponent<Rigidbody2D>();
            //_distanceJoint.distance = Vector2.Distance(transform.position, _closestGrapplePoint.transform.position);

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
        if (_jump.WasPressedThisFrame() && !_isGrounded)
        {
            // launch player towards closest grapple point
            if (_closestGrapplePoint != null)
            {
                StartCoroutine(GrappleCoroutine());
                _isGrappling = true;
            }
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
        float originalGravityScale = _rigidbody.gravityScale;
        while (Player.transform.position != desiredGrapplePosition) 
        {
            _rigidbody.linearVelocity = Vector2.zero; // reset velocity to prevent physics interference
            _rigidbody.gravityScale = 0f; // disable gravity while grappling
            Player.transform.position = Vector3.MoveTowards(transform.position, desiredGrapplePosition, GrappleTime * Time.deltaTime);
            yield return null;
        }
        _rigidbody.gravityScale = originalGravityScale; // re-enable gravity after grappling
        LaunchPlayer();
        _isGrappling = false;
    }

    private void LaunchPlayer()
    {
        _rigidbody.AddForce(Vector2.up * LaunchForce, ForceMode2D.Impulse);
    }

    public bool IsGrappling()
    {
        return _isGrappling;
    }
}
