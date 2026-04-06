using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public UnityEvent OnBreak;
    private Collider2D _blockCollider;

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
        //Debug.Log("Player is air jumping: " + isAirJumping);
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
            //Debug.Log("Is Air jumping: " + _isAirJumping);
            // Ensure there's at least one contact, then use GetContact instead of contacts
            if (collision.GetContact(0).normal.y > 0 && !_isDescending)
            {
                // If the player is jumping, break the object
                OnBreak?.Invoke();
                gameObject.SetActive(false);

                //Break();
            }
        }
        if (collision.gameObject.CompareTag("Explosion"))
        {
            OnBreak?.Invoke();
            gameObject.SetActive(false);

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Is Air jumping: " + _isAirJumping);
            if (collision.GetComponent<Jump>().GetIsAirJumping() && !_isDescending)
            {
                OnBreak?.Invoke();
                gameObject.SetActive(false);

            }
        }

        if (collision.gameObject.CompareTag("Explosion"))
        {
            OnBreak?.Invoke();
            gameObject.SetActive(false);

        }


    }

    public void InstantiatePrefab(GameObject prefab)
    {
        Vector3 spawnPosition = transform.position;
        // instantiate a prefab outside of its parent heirarchy
        Instantiate(prefab, spawnPosition, Quaternion.identity, null);
    }


}
