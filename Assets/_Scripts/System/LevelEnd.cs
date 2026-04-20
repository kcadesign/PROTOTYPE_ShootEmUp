using System;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    public static event Action<int> OnPlayerEnterLevelEnd;
    public UnityEvent OnLevelEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnLevelEnd?.Invoke();
            //collision.GetComponent<Jump>().enabled = false;
            //collision.GetComponent<Rigidbody2D>().gravityScale = -1f;
        }
    }

    public void MoveToNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        OnPlayerEnterLevelEnd?.Invoke(nextSceneIndex);
    }
}
