using UnityEngine;

public class HandlePlayerDeath : MonoBehaviour
{
    private PlayerHealth _health;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (_health.GetHealth() == 0)
        {
            // Handle death logic here (e.g., play animation, disable controls, etc.)
            gameObject.SetActive(false);
        }
    }
}
