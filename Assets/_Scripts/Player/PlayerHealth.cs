using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> OnCurrentHealthChanged; // Event to notify when health changes
    public static event Action<int> OnMaxHealthChanged; // Event to notify when the player dies

    public int MaxHealth = 3;
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = MaxHealth;
        OnCurrentHealthChanged?.Invoke(_currentHealth); // Notify initial health
        OnMaxHealthChanged?.Invoke(MaxHealth); // Notify initial max health
    }

    public void Damage(int damageAmount)
    {
        OnCurrentHealthChanged?.Invoke(_currentHealth);

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
