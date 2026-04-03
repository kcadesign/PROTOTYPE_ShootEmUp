using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action<int> OnCurrentHealthChanged; // Event to notify when health changes
    public static event Action<int> OnMaxHealthChanged; // Event to notify when the player dies

    public int MaxHealth = 3;
    private int _currentHealth;

    private void Start()
    {
        _currentHealth = MaxHealth;
        OnCurrentHealthChanged?.Invoke(_currentHealth); // Notify initial health
        OnMaxHealthChanged?.Invoke(MaxHealth); // Notify initial max health
    }

    public void Damage(int damageAmount)
    {

        if (_currentHealth <= 0)
        {
            Debug.Log($"{gameObject.name} has died.");
            return; // Already at 0 health, do nothing
        }
        else if (_currentHealth != 0)
        {
            _currentHealth -= damageAmount;
            Debug.Log($"{gameObject.name} took {damageAmount} damage. Current health: {_currentHealth}");
            OnCurrentHealthChanged?.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                Debug.Log($"{gameObject.name} has died.");
                return; // Already at 0 health, do nothing
            }
        }
    }

    public void IncreaseMaxHealth()
    {
        // Increase max health by 1 but also heal 1
        MaxHealth += 1;
        OnMaxHealthChanged?.Invoke(MaxHealth);
        Heal(1); // Heal the player by 1 to increase current health
    }

    public void Heal(int healAmount)
    {
        if (_currentHealth == MaxHealth)
        {
            Debug.Log($"{gameObject.name} is already at max health.");
            return; // Already at max health, do nothing
        }
        else if (_currentHealth != MaxHealth)
        {
            _currentHealth += healAmount;
            if (_currentHealth > MaxHealth)
            {
                _currentHealth = MaxHealth; // Cap health at max
            }
            Debug.Log($"{gameObject.name} healed {healAmount} health. Current health: {_currentHealth}");
            OnCurrentHealthChanged?.Invoke(_currentHealth);
        }
    }

    public int GetHealth()
    {
        return _currentHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        MaxHealth = maxHealth;
    }
}
