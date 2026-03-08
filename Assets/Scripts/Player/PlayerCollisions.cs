using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerHealth _health;
    public Grapple PlayerGrapple;
    private Collider2D _playerBodyCollider;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
        _playerBodyCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (_playerBodyCollider.IsTouching(collision))
        {
            if (collision.gameObject.CompareTag("Enemy") && !PlayerGrapple.IsGrappling())
            {
                Debug.Log("Player Body Collided with: " + collision.gameObject.name);
                if (_health != null)
                {
                    _health.Damage(1); // Assuming the player has a Health component
                                       // push player away after damaging them
                }
            }
            else if (collision.gameObject.CompareTag("Enemy") && PlayerGrapple.IsGrappling())
            {
                if (collision.GetComponent<Health>() != null)
                {
                    collision.GetComponent<Health>().Damage(1); // Assuming the enemy has a Health component
                }
            }
        }
    }

}
