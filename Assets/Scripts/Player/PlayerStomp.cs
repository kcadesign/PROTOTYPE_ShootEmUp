using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    [Header("References")]
    public Jump JumpScript;
    public Grapple GrappleScript;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    private Collider2D _stompZoneCollider; // Assign the stomp zone collider in the inspector

    //public float BounceForce = 10f; // Adjust the bounce force as needed

    private void Awake()
    {
        _stompZoneCollider = GetComponent<Collider2D>(); // Assuming the stomp zone collider is on the same GameObject
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (_stompZoneCollider.IsTouching(collision))
        {
            if (collision.TryGetComponent(out WeakPoint weakPoint) && !GrappleScript.IsGrappling() && _playerRigidbody.linearVelocityY <= 0f)
            {
                Debug.Log("Player Stomp Collided with: " + collision.gameObject.name);
                //Debug.Log("Player stomped on an enemy!");
                // debug the collision object name
                JumpScript.DoJump();

                Health enemyHealth = collision.GetComponentInParent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(1); // Assuming the enemy has a Health component
                }
            }
        }
    }
}
