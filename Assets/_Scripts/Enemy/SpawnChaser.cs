using UnityEngine;

public class SpawnChaser : MonoBehaviour
{
    public GameObject ChaserPrefab;
    private GameObject _chaser;
    public Vector3 SpawnPosition;
    //private Collider2D _playerCollider;

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        if (state == HandleGameState.GameState.LevelStart)
        {
            if (_chaser == null)
            {
                Debug.Log("Chaser not found. Spawning chaser at spawn position.");
                InstantiateChaser();
            }
            else if (_chaser != null)
            {
                Debug.Log("Chaser already exists. Moving chaser to spawn position.");
                _chaser.transform.position = SpawnPosition;

                if (!_chaser.activeSelf)
                {
                    Debug.Log("Chaser inactive. Setting active now and resetting health.");
                    _chaser.SetActive(true);
                }
            }
        }
        else if (state == HandleGameState.GameState.LevelEnd)
        {
            if (_chaser != null)
            {
                Debug.Log("Level ended. Deactivating chaser.");
                _chaser.SetActive(false);
            }
        }
        else if (state == HandleGameState.GameState.PreGameMenu)
        {
            if (_chaser != null)
            {
                Debug.Log("PreGameMenu state. Deactivating chaser.");
                _chaser.SetActive(false);
            }
        }
    }

    private void InstantiateChaser()
    {
        _chaser = Instantiate(ChaserPrefab, SpawnPosition, Quaternion.identity);
        // set the chaser to not destroy on load so it persists across scenes
        DontDestroyOnLoad(_chaser);
    }
}
