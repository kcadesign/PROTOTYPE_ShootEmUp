using UnityEngine;

public class ChaserCollisions : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.TryGetComponent(out Health healthComponent);
            if (healthComponent != null)
            {
                healthComponent.Damage(999);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.TryGetComponent(out HandleDeath handleDeathComponent);
            if (handleDeathComponent != null)
            {
                handleDeathComponent.CollateralDamage();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.TryGetComponent(out HandleDeath handleDeathComponent);
            if (handleDeathComponent != null)
            {
                handleDeathComponent.CollateralDamage();
            }
        }
    }
}
