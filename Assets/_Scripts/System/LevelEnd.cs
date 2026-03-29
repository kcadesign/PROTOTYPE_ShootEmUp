using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;

public class LevelEnd : MonoBehaviour
{
    public static event Action<int> OnPlayerEnterLevelEnd;
    public UnityEvent OnLevelEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnLevelEnd?.Invoke();
        }
    }

    public void ChangeToScene(int sceneIndex)
    {
        OnPlayerEnterLevelEnd?.Invoke(sceneIndex);
    }
}
