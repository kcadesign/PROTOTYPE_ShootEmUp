using System;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    public static event Action<int> OnCurrentStaminaChanged; // Event to notify when stamina changes
    public static event Action<int> OnMaxStaminaChanged;

    public int MaxStamina = 1;
    [SerializeField] private int _currentStamina;

    private void OnEnable()
    {
        PlayerGround.OnGround += PlayerGround_OnGround;
        HandleDeath.OnEnemyDeath += HandleDeath_OnEnemyDeath;
    }

    private void OnDisable()
    {
        PlayerGround.OnGround -= PlayerGround_OnGround;
        HandleDeath.OnEnemyDeath -= HandleDeath_OnEnemyDeath;
    }

    private void PlayerGround_OnGround(bool onGround)
    {
        if (onGround)
        {
            ResetStamina();
        }
    }

    private void HandleDeath_OnEnemyDeath()
    {
        RestoreStamina(1);
    }

    private void Start()
    {
        _currentStamina = MaxStamina;
        OnCurrentStaminaChanged?.Invoke(_currentStamina); // Notify initial stamina
        OnMaxStaminaChanged?.Invoke(MaxStamina); // Notify initial max stamina
    }

    public void UseStamina(int amount)
    {
        if (_currentStamina <= 0)
        {
            Debug.Log($"{gameObject.name} is out of stamina.");
            return; // Already at 0 stamina, do nothing
        }
        else if (_currentStamina != 0)
        {
            _currentStamina -= amount;
            Debug.Log($"{gameObject.name} used {amount} stamina. Current stamina: {_currentStamina}");
            OnCurrentStaminaChanged?.Invoke(_currentStamina);

            if (_currentStamina <= 0)
            {
                Debug.Log($"{gameObject.name} is out of stamina.");
                return; // Already at 0 stamina, do nothing
            }
        }
    }

    public void IncreaseMaxStamina()
    {
        // Increase max stamina by 1 but also restore 1
        MaxStamina += 1;
        OnMaxStaminaChanged?.Invoke(MaxStamina);
        RestoreStamina(1); // Restore the player by 1 to increase current stamina
    }

    public void SetMaxStamina(int amount)
    {
        MaxStamina = amount;
        OnMaxStaminaChanged?.Invoke(MaxStamina);
        if (_currentStamina > MaxStamina)
        {
            _currentStamina = MaxStamina; // Ensure current stamina does not exceed max
            OnCurrentStaminaChanged?.Invoke(_currentStamina);
        }
    }

    public void RestoreStamina(int restoreAmount)
    {
        if (_currentStamina == MaxStamina)
        {
            Debug.Log($"{gameObject.name} is already at max stamina.");
            return; // Already at max stamina, do nothing
        }
        else if (_currentStamina != MaxStamina)
        {
            _currentStamina += restoreAmount;
            if (_currentStamina > MaxStamina)
            {
                _currentStamina = MaxStamina; // Cap stamina at max
            }
            Debug.Log($"{gameObject.name} restored {restoreAmount} stamina. Current stamina: {_currentStamina}");
            OnCurrentStaminaChanged?.Invoke(_currentStamina);
        }
    }

    public int GetStamina()
    {
        return _currentStamina;
    }

    //public void SetMaxStamina(int maxStamina)
    //{
    //    MaxStamina = maxStamina;
    //}

    public void SetStaminaZero()
    {
        _currentStamina = 0;
        OnCurrentStaminaChanged?.Invoke(_currentStamina); // Notify stamina change
    }

    public void ResetStamina()
    {
        _currentStamina = MaxStamina;
        OnCurrentStaminaChanged?.Invoke(_currentStamina); // Notify stamina reset
    }

}
