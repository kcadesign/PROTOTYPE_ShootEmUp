using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private Jump _playerJump;
    private PlayerHealth _playerHealth;
    public Grapple PlayerGrapple;
    private Rigidbody2D _playerRigidbody;
    private Collider2D _playerBodyCollider;

    [Header("Knockback Settings")]
    public float KnockbackDuration = 0.5f;
    public float KnockbackPower = 5f;
    private bool _isKnockbackActive = false;

    [Header("Launch Settings")]
    public float LaunchMultiplier = 1.0f;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerJump = GetComponent<Jump>();
        _playerHealth = GetComponent<PlayerHealth>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerBodyCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.IsTouching(_playerBodyCollider)) return;
        if (collision.gameObject.CompareTag("Enemy")
            && (PlayerGrapple.GetIsGrappling() || _playerJump.GetIsAirJumping())
            && _playerRigidbody.linearVelocityY > 0)
        {
            if (collision.GetComponent<Health>() != null)
            {
                collision.GetComponent<Health>().Damage(1); // Assuming the enemy has a Health component
            }
            _playerJump.DoJump(LaunchMultiplier);
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player Body Collided with: " + collision.gameObject.name);
            if (_playerHealth != null)
            {
                _playerHealth.Damage(1);
                // push the player away in the opposite direction of the collision
                Vector2 contactPoint = collision.ClosestPoint(transform.position);
                Vector2 pushDirection = (Vector2)(transform.position) - contactPoint;
                pushDirection.Normalize();
                StartCoroutine(Knockback(KnockbackDuration, KnockbackPower, pushDirection));
            }
        }

        if (collision.CompareTag("Chaser") || collision.CompareTag("Spike"))
        {
            _playerHealth.Damage(999);
        }

        if (collision.CompareTag("Explosion"))
        {
            Debug.Log("Launching from explosion");
            _playerJump.DoAirJump(LaunchMultiplier);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")
            && (PlayerGrapple.GetIsGrappling() || _playerJump.GetIsAirJumping())
            && _playerRigidbody.linearVelocityY > 0)
        {
            //collision.GetComponent<Health>()?.Damage(1); // Assuming the enemy has a Health component
            collision.gameObject.GetComponent<Health>()?.Damage(1);
            _playerJump.DoJump(LaunchMultiplier);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player Body Collided with: " + collision.gameObject.name);
            if (_playerHealth != null)
            {
                _playerHealth.Damage(1);
                // push the player away in the opposite direction of the collision
                Vector2 contactPoint = collision.GetContact(0).point;
                Vector2 pushDirection = (Vector2)(transform.position) - contactPoint;
                pushDirection.Normalize();
                StartCoroutine(Knockback(KnockbackDuration, KnockbackPower, pushDirection));
            }
        }

    }

    private IEnumerator Knockback(float duration, float power, Vector2 direction)
    {
        _isKnockbackActive = true;
        _playerMovement.DisableMovementInput();

        _playerRigidbody.linearVelocity = Vector2.zero;
        _playerRigidbody.AddForce(direction * power, ForceMode2D.Impulse);

        float timer = 0f;
        // Use FixedUpdate timing so we wait alongside the physics simulation
        while (timer < duration)
        {
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        _playerMovement.EnableMovementInput();
        _isKnockbackActive = false;
    }

    public bool GetIsKnockbackActive()
    {
        return _isKnockbackActive;
    }
}
