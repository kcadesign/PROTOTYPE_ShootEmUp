using System.Collections;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerHealth _health;
    public Grapple PlayerGrapple;
    private Rigidbody2D _playerRigidbody;
    private Collider2D _playerBodyCollider;

    [Header("Knockback Settings")]
    public float KnockbackDuration = 0.5f;
    public float KnockbackPower = 5f;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _health = GetComponent<PlayerHealth>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerBodyCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (_playerBodyCollider.IsTouching(collision))
        {
            if (collision.gameObject.CompareTag("Enemy") && !PlayerGrapple.GetIsGrappling())
            {
                Debug.Log("Player Body Collided with: " + collision.gameObject.name);
                if (_health != null)
                {
                    _health.Damage(1);
                    // push the player away in the opposite direction of the collision
                    Vector2 pushDirection = (transform.position - collision.transform.position).normalized;
                    StartCoroutine(Knockback(KnockbackDuration, KnockbackPower, pushDirection));
                }
            }
            else if (collision.gameObject.CompareTag("Enemy") && PlayerGrapple.GetIsGrappling())
            {
                if (collision.GetComponent<Health>() != null)
                {
                    collision.GetComponent<Health>().Damage(1); // Assuming the enemy has a Health component
                }
            }
        }
    }

    private IEnumerator Knockback(float duration, float power, Vector2 direction)
    {
        float timer = 0;
        while (timer < duration)
        {
            _playerRigidbody.AddForce(direction * power, ForceMode2D.Impulse);
            _playerMovement.DisableMovementInput();
            timer += Time.deltaTime;
            yield return null;
        }
        _playerMovement.EnableMovementInput();
    }
}
