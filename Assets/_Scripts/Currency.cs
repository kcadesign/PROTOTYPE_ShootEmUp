using System;
using UnityEngine;
using UnityEngine.Events;

public class Currency : MonoBehaviour
{
    public UnityEvent OnCollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollect?.Invoke();
            gameObject.SetActive(false);
        }
    }

    //public void CollectCurrency(int amount)
    //{

    //}

    public void InstantiatePrefab(GameObject prefab)
    {
        Vector3 spawnPosition = transform.position;
        // instantiate a prefab outside of its parent heirarchy
        Instantiate(prefab, spawnPosition, Quaternion.identity, null);
    }

}
