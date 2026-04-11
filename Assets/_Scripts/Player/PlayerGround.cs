using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    [SerializeField] private bool _onGround;
    private Rigidbody2D _playerRigidbody;

    [Header("Collider Settings")]
    [SerializeField] private float groundLength = 0.95f;
    [SerializeField] private Vector3 colliderOffset;
    public float RayGap = 0.5f;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;

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

        //RaycastHit2D hit = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, _groundLayer);
        //_onGround = hit.collider != null;

        // create 2 downwards raycasts and check both for ground detection, this is to prevent the player from being detected as in the air when only one of the raycasts hits the ground
        RaycastHit2D hit1 = Physics2D.Raycast(transform.position + colliderOffset + Vector3.left * RayGap, Vector2.down, groundLength, _groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position + colliderOffset + Vector3.right * RayGap, Vector2.down, groundLength, _groundLayer);
        _onGround = hit1.collider != null || hit2.collider != null;

        //if (_onGround)
        //{
        //    Debug.Log("Ground detected: " + hit.collider.name);
        //}
    }

    private void OnDrawGizmos()
    {
        //Draw the ground colliders on screen for debug purposes
        if (_onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
        //Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + colliderOffset + Vector3.left * RayGap, transform.position + colliderOffset + Vector3.left * RayGap + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position + colliderOffset + Vector3.right * RayGap, transform.position + colliderOffset + Vector3.right * RayGap + Vector3.down * groundLength);
    }

    //Send ground detection to other scripts
    public bool GetOnGround() { return _onGround; }
}
