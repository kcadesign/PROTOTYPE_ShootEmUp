using System;
using System.Collections;
using UnityEngine;

public class PlayerBodyCollisions : MonoBehaviour
{
    public static event Action OnDamageCollision;
    private PlayerMovement _playerMovement;
    private Jump _playerJump;
    private PlayerHealth _playerHealth;
    public Grapple PlayerGrapple;
    private Rigidbody2D _playerRigidbody;
    private Collider2D _playerBodyCollider;
    public Shield Shield;

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

    private void OnEnable()
    {
        EnemyLaunchMultiplier.OnMultiplyEnemyLaunch += EnemyLaunchMultiplier_OnMultiplyEnemyLaunch;
    }

    private void OnDisable()
    {
        EnemyLaunchMultiplier.OnMultiplyEnemyLaunch -= EnemyLaunchMultiplier_OnMultiplyEnemyLaunch;
    }

    private void EnemyLaunchMultiplier_OnMultiplyEnemyLaunch()
    {
        MultiplyLaunchMultiplier(1.5f);
        Debug.Log("Launch multiplier increased to: " + LaunchMultiplier);
    }
    // --------------------------------------------------------------------------------
    // FIX: Where is the separation between this collision script and the stomp script?
    // This script should be called handle player body collisions and only deal with things touching the player body
    // The stomp script should only deal with collisions with the stomp zone
    // --------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.IsTouching(_playerBodyCollider)) return;
        // if the player is boost jumping...
        if (collision.gameObject.CompareTag("Enemy")
            && _playerJump.GetIsAirJumping()
            && _playerRigidbody.linearVelocityY > 0)
        {
            if (collision.GetComponent<Health>() != null)
            {
                Debug.Log("Player DEALT damage - Collided with: " + collision.gameObject.name);

                collision.GetComponent<Health>().Damage(1);
                _playerJump.DoJump(LaunchMultiplier);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            if (_playerHealth != null)
            {
                if (!Shield.GetIsShieldActive())
                {
                    Debug.Log("Player RECEIVED damage - Collided with TRIGGER: " + collision.gameObject.name);
                    _playerHealth.Damage(1);
                }
                OnDamageCollision?.Invoke();
                // push the player away in the opposite direction of the collision
                Vector2 contactPoint = collision.ClosestPoint(transform.position);
                Vector2 pushDirection = (Vector2)(transform.position) - contactPoint;
                pushDirection.Normalize();

                if (!gameObject.activeSelf) return;
                StartCoroutine(Knockback(KnockbackDuration, KnockbackPower, pushDirection));
            }
        }

        if (collision.CompareTag("Chaser") || collision.CompareTag("Spike"))
        {
            _playerHealth.SetHealthZero();
        }

        if (collision.CompareTag("Explosion"))
        {
            //Debug.Log("Launching from explosion");
            _playerJump.DoAirJump(LaunchMultiplier);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _playerJump.GetIsAirJumping()
            /*&& _playerRigidbody.linearVelocityY > 0*/)
        {
            if (collision.gameObject.GetComponent<Health>() != null)
            {
                collision.gameObject.GetComponent<Health>().Damage(1);
                _playerJump.DoJump(LaunchMultiplier);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            if (_playerHealth != null)
            {
                if (!Shield.GetIsShieldActive())
                {
                    Debug.Log("Player RECEIVED damage - Collided with COLLIDER: " + collision.gameObject.name);
                    _playerHealth.Damage(1);
                }
                OnDamageCollision?.Invoke();
                // push the player away in the opposite direction of the collision
                Vector2 contactPoint = collision.GetContact(0).point;
                Vector2 pushDirection = (Vector2)(transform.position) - contactPoint;
                pushDirection.Normalize();

                if (!gameObject.activeSelf) return;
                StartCoroutine(Knockback(KnockbackDuration, KnockbackPower, pushDirection));
            }
        }

        if (collision.gameObject.CompareTag("Chaser") || collision.gameObject.CompareTag("Spike"))
        {
            _playerHealth.SetHealthZero();
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

    public void MultiplyLaunchMultiplier(float amount)
    {
        LaunchMultiplier *= amount;
    }
}
