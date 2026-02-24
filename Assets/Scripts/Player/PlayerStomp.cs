using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _playerRigidbody;

    public float BounceForce = 10f; // Adjust the bounce force as needed

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // debug the collision object name
            //Debug.Log("Collided with: " + collision.gameObject.name);

            _playerRigidbody.linearVelocity = new Vector2(_playerRigidbody.linearVelocity.x, BounceForce); // Adjust the bounce force as needed
            // You can also add code here to damage the enemy or trigger an animation
            Health enemyHealth = collision.GetComponentInParent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(1); // Assuming the enemy has a Health component
            }
        }
    }
}
