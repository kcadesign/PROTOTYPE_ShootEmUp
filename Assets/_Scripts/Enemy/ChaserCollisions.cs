using UnityEngine;

public class ChaserCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            collision.TryGetComponent(out Health healthComponent);
            if (healthComponent != null)
            {
                healthComponent.Damage(999);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().Damage(999);
        }
    }
}
