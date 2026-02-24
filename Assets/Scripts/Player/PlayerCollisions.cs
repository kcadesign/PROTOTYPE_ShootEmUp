using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private Health _health;
    public Grapple PlayerGrapple;

    private void Awake()
    {
        _health = GetComponent<Health>();
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
            Health enemyHealth = collision.gameObject.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(1); // Assuming the enemy also has a Health component
            }
        }
    }

}
