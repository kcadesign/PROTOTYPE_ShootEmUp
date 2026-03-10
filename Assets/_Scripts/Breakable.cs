using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject BreakEffectPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ensure there's at least one contact, then use GetContact instead of contacts
            if (collision.GetContact(0).normal.y > 0)
            {
                // If the player is jumping, break the object
                Break();
            }
        }
    }

    private void Break()
    {
        // Instantiate the break effect at the position of the breakable object
        Instantiate(BreakEffectPrefab, transform.position, Quaternion.identity);
        // Destroy the breakable object
        Destroy(gameObject);
    }
}
