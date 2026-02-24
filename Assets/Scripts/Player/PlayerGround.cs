using UnityEngine;

public class PlayerGround : MonoBehaviour
{
    private bool _onGround;

    [Header("Collider Settings")]
    [SerializeField][Tooltip("Length of the ground-checking collider")] private float groundLength = 0.95f;
    [SerializeField][Tooltip("Distance between the ground-checking colliders")] private Vector3 colliderOffset;

    [Header("Layer Masks")]
    [SerializeField][Tooltip("Which layers are read as the ground")] private LayerMask groundLayer;


    private void Update()
    {
        //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
        _onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer);
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
