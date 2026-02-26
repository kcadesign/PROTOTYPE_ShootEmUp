using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerHealth _health;
    public Grapple PlayerGrapple;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !PlayerGrapple.IsGrappling())
        {
            if (_health != null)
            {
                _health.Damage(1); // Assuming the player has a Health component
            }
        }
        else if (collision.gameObject.CompareTag("Enemy") && PlayerGrapple.IsGrappling())
        {
            PlayerHealth enemyHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(1); // Assuming the enemy also has a Health component
            }
        }
    }

}
