using UnityEngine;

public class PlayerStomp : MonoBehaviour
{
    [Header("References")]
    public Jump JumpScript;
    public Grapple GrappleScript;
    [SerializeField] private Rigidbody2D _playerRigidbody;
    private Collider2D _stompZoneCollider;

    public float LaunchMultiplier = 1.0f;

    //public float BounceForce = 10f; // Adjust the bounce force as needed

    private void Awake()
    {
        _stompZoneCollider = GetComponent<Collider2D>(); // Assuming the stomp zone collider is on the same GameObject
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
        Debug.Log("Enemy launch multiplier event received in PlayerStomp.");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (_stompZoneCollider.IsTouching(collision))
        {
            if (collision.TryGetComponent(out WeakPoint weakPoint) && !GrappleScript.GetIsGrappling() && _playerRigidbody.linearVelocityY < 0f)
            {
                Debug.Log("Player Stomp Collided with: " + collision.gameObject.name);
                //Debug.Log("Player stomped on an enemy!");
                // debug the collision object name
                JumpScript.DoJump(LaunchMultiplier);
                JumpScript.RenewAirJumps(1);

                Health enemyHealth = collision.GetComponentInParent<Health>();
                if (enemyHealth != null)
                {
                    enemyHealth.Damage(1); // Assuming the enemy has a Health component
                }
            }
        }
    }

    public void MultiplyLaunchMultiplier(float amount)
    {
        LaunchMultiplier *= amount;
    }

}
