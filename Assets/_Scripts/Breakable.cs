using UnityEngine;

public class Breakable : MonoBehaviour
{
    private Collider2D _blockCollider;
    public GameObject BreakEffectPrefab;

    private bool _isAirJumping;
    private bool _isDescending;

    private void Awake()
    {
        _blockCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        Jump.OnAirJump += Jump_OnAirJump;
        Jump.OnPlayerDescending += Jump_OnPlayerDescending;
    }

    private void OnDisable()
    {
        Jump.OnAirJump -= Jump_OnAirJump;
        Jump.OnPlayerDescending -= Jump_OnPlayerDescending;
    }

    private void Jump_OnAirJump(bool isAirJumping)
    {
        _isAirJumping = isAirJumping;

    }

    private void Jump_OnPlayerDescending(bool isDescending)
    {
        _isDescending = isDescending;

        if (_isAirJumping && !_isDescending)
        {
            _blockCollider.isTrigger = true; // Enable trigger to allow passing through
        }
        else
        {
            _blockCollider.isTrigger = false; // Disable trigger to restore normal collision
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ensure there's at least one contact, then use GetContact instead of contacts
            if (collision.GetContact(0).normal.y > 0 && !_isDescending)
            {
                // If the player is jumping, break the object
                Break();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.GetComponent<Jump>().GetAirJumping() && !_isDescending)
            {
                Break();
            }
        }
    }

    public void Break()
    {
        // Instantiate the break effect at the position of the breakable object
        Instantiate(BreakEffectPrefab, transform.position, Quaternion.identity);
        // Destroy the breakable object
        Destroy(gameObject);
    }


}
