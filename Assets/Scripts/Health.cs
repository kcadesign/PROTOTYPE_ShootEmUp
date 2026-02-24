using UnityEngine;

public class Health : MonoBehaviour
{
    public int MaxHealth = 3;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public void Damage(int damageAmount)
    {
        if (_currentHealth == 0)
        {
            Debug.Log($"{gameObject.name} has died.");
            return; // Already at 0 health, do nothing
        }
        else if (_currentHealth != 0)
        {
            _currentHealth -= damageAmount;
            Debug.Log($"{gameObject.name} took {damageAmount} damage. Current health: {_currentHealth}");

            if (_currentHealth == 0)
            {
                Debug.Log($"{gameObject.name} has died.");
                return; // Already at 0 health, do nothing
            }
        }
    }

    public int GetHealth()
    {
                return _currentHealth;
    }
}
