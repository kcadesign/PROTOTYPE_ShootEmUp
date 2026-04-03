using UnityEngine;

public class Deactivate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponentInChildren<Grapple>().GetIsGrappling())
            {
                gameObject.SetActive(false);
            }
        }
    }
}
