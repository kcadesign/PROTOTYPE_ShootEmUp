using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    [SerializeField] private bool _onGround;
    private Rigidbody2D _playerRigidbody;

    [Header("Collider Settings")]
    [SerializeField] private float groundLength = 0.95f;
    [SerializeField] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Use the correct Rigidbody2D property and a small epsilon to avoid noise
        if (_playerRigidbody.linearVelocityY > 0.01f)
        {
            _onGround = false;
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer);
        _onGround = hit.collider != null;

        //if (_onGround)
        //{
        //    Debug.Log("Ground detected: " + hit.collider.name);
        //}
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (_onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return _onGround; }
}
