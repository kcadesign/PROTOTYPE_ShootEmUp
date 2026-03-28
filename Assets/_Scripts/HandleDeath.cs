using UnityEngine;
using UnityEngine.Events;

public class HandleDeath : MonoBehaviour
{
    public UnityEvent OnDeath;
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (_health.GetHealth() <= 0)
        {
            OnDeath?.Invoke();
            // Handle death logic here (e.g., play animation, disable controls, etc.)
            gameObject.SetActive(false);
        }
    }

    public bool GetAlive()
    {
        return _health.GetHealth() > 0;
    }

    public void InstantiatePrefab(GameObject prefab)
    {
        Vector3 spawnPosition = transform.position;
        // instantiate a prefab outside of its parent heirarchy
        Instantiate(prefab, spawnPosition, Quaternion.identity, null);
    }
}
