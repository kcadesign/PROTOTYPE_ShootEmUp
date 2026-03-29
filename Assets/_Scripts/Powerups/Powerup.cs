using UnityEngine;
using UnityEngine.Events;

public class Powerup : MonoBehaviour
{
    public UnityEvent OnCollect;
    //public PowerupEffect PowerupEffect;

    private Collider2D Collision;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collision = collision;
            OnCollect?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void ApplyEffect(PowerupEffect powerupEffect)
    {
        powerupEffect.Apply(Collision.gameObject);
    }

    public void InstantiatePrefab(GameObject prefab)
    {
        Vector3 spawnPosition = transform.position;
        // instantiate a prefab outside of its parent heirarchy
        Instantiate(prefab, spawnPosition, Quaternion.identity, null);
    }

}
