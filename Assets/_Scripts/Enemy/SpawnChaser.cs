using UnityEngine;

public class SpawnChaser : MonoBehaviour
{
    public GameObject ChaserPrefab;
    private GameObject _chaser;
    public Vector3 DefaultSpawnPosition;
    public Vector3 BossLevelSpawnPosition;
    //private Collider2D _playerCollider;

    private bool _isBossFight = false;

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
        SignalBossLevel.OnExitBossLevel += SignalBossLevel_OnExitBossLevel;
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;
        SignalBossLevel.OnExitBossLevel -= SignalBossLevel_OnExitBossLevel;
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        if (state == HandleGameState.GameState.LevelStart)
        {
            if (_chaser == null && !_isBossFight)
            {
                Debug.Log("Chaser not found. Spawning chaser at spawn position.");
                InstantiateChaser(DefaultSpawnPosition);
            }
            else if (_chaser != null && !_isBossFight)
            {
                _chaser.GetComponent<Chase>().SetPosition(DefaultSpawnPosition);
                if (!_chaser.activeSelf)
                {
                    Debug.Log("Chaser inactive. Setting active now and resetting health.");
                    _chaser.SetActive(true);
                }
            }
        }
        else if (state == HandleGameState.GameState.BossFight)
        {
            SetBossFight(true);
            if (_chaser == null)
            {
                Debug.Log("Chaser not found. Spawning chaser at spawn position.");
                InstantiateChaser(BossLevelSpawnPosition);
                _chaser.GetComponent<Chase>().SetBossFight(true);
            }
            else if (_chaser != null)
            {
                Debug.Log("Chaser already exists. Moving chaser to spawn position.");
                _chaser.GetComponent<Chase>().SetPosition(BossLevelSpawnPosition);

                if (!_chaser.activeSelf)
                {
                    Debug.Log("Chaser inactive. Setting active now and resetting health.");
                    _chaser.SetActive(true);
                }
            }
        }
        //else if (state == HandleGameState.GameState.LevelEnd)
        //{
        //    if (_chaser != null)
        //    {
        //        //Debug.Log("Level ended. Deactivating chaser.");
        //        //_chaser.SetActive(false);
        //        //_chaser.GetComponent<Chase>().PauseChaserMovement();
        //    }
        //}
        else if (state == HandleGameState.GameState.PreGameMenu)
        {
            if (_chaser != null)
            {
                Debug.Log("PreGameMenu state. Deactivating chaser.");
                _chaser.SetActive(false);
            }
        }
    }

    private void SignalBossLevel_OnExitBossLevel()
    {
        SetBossFight(false);
    }

    private void InstantiateChaser(Vector3 spawnPosition)
    {
        _chaser = Instantiate(ChaserPrefab, spawnPosition, Quaternion.identity);
        // set the chaser to not destroy on load so it persists across scenes
        DontDestroyOnLoad(_chaser);
    }

    private void SetBossFight(bool isBossFight)
    {
        _isBossFight = isBossFight;
    }
}
