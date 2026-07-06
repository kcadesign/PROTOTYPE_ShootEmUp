using UnityEngine;

public class TriggerCollision : MonoBehaviour
{
    private Vector3 _collisionPosition;

    private void Awake()
    {
        // Initialize to this object's position so there is no implicit (0,0,0) target
        _collisionPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collisionPosition = collision.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collisionPosition = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _collisionPosition = transform.position;
        }
    }

    public Vector3 GetTargetPosition()
    {
        return _collisionPosition;
    }
}
