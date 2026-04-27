using System;
using UnityEngine;
using UnityEngine.Events;

public class HandlePlayerDeath : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    public UnityEvent OnDeath;
    private PlayerHealth _health;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
            PlayerHealth.OnCurrentHealthChanged += PlayerHealth_OnCurrentHealthChanged;
    }

    private void OnDisable()
    {
            PlayerHealth.OnCurrentHealthChanged -= PlayerHealth_OnCurrentHealthChanged;
    }

    private void PlayerHealth_OnCurrentHealthChanged(int currentHealth)
    {
        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            OnPlayerDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void InstantiatePrefab(GameObject prefab)
    {
        Vector3 spawnPosition = transform.position;
        // instantiate a prefab outside of its parent heirarchy
        Instantiate(prefab, spawnPosition, Quaternion.identity, null);
    }

}
