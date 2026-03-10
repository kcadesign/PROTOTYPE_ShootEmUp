using UnityEngine;

public class HandleDeath : MonoBehaviour
{
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
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
