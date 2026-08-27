using System.Collections;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    private GameObject _player;
    public Vector3 SpawnPosition;
    private Collider2D _playerCollider;

    private void OnEnable()
    {
        //SceneController.OnLevelLoaded += SceneController_OnLevelLoaded;
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
    }

    private void OnDisable()
    {
        //SceneController.OnLevelLoaded -= SceneController_OnLevelLoaded;
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;
    }

    //private void SceneController_OnLevelLoaded()
    //{
    //    if(_player == null)
    //    {
    //        Debug.Log("Player not found. Spawning player at spawn position.");
    //        InstantiatePlayer();
    //    }
    //    else if (_player != null)
    //    {
    //        Debug.Log("Player already exists. Moving player to spawn position.");
    //        _player.transform.position = SpawnPosition;
            
    //        if (!_player.activeSelf)
    //        {
    //            Debug.Log("Player inactive. Setting active now and resetting health.");
    //            _player.SetActive(true);
    //            _player.GetComponent<PlayerHealth>()?.ResetHealth(); // Reset health when respawning
    //        }
    //    }

    //}

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        if (state == HandleGameState.GameState.LevelStart)
        {
            if (_player == null)
            {
                Debug.Log("Player not found. Spawning player at spawn position.");
                InstantiatePlayer();
            }
            else if (_player != null)
            {
                Debug.Log("Player already exists. Moving player to spawn position.");
                _player.transform.position = SpawnPosition;
                _playerCollider = _player.GetComponent<Collider2D>();
                _playerCollider.enabled = true; // Enable the player's collider when the level starts
                Invoke(nameof(ReleaseMovementConstraints), 0.25f);

                if (!_player.activeSelf)
                {
                    Debug.Log("Player inactive. Setting active now and resetting health.");
                    _player.SetActive(true);
                    _player.GetComponent<PlayerHealth>()?.ResetHealth(); // Reset health when respawning
                }
            }
        }
        else if (state == HandleGameState.GameState.LevelEnd)
        {
            if (_player != null)
            {
                _playerCollider = _player.GetComponent<Collider2D>();
                _playerCollider.enabled = false; // Disable the player's collider when the level ends
                Invoke(nameof(ConstrainMovement), 0.25f);
            }
        }
    }

    private void InstantiatePlayer()
    {
        _player = Instantiate(PlayerPrefab, SpawnPosition, Quaternion.identity);
        // set the player to not destroy on load so it persists across scenes
        DontDestroyOnLoad(_player);
    }

    private void ConstrainMovement()
    {
        // constrain player rigidbody movement X and Y
        Rigidbody2D playerRigidbody = _player.GetComponent<Rigidbody2D>();
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
    }

    private void ReleaseMovementConstraints()
    {
        Rigidbody2D playerRigidbody = _player.GetComponent<Rigidbody2D>();
        playerRigidbody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
    }
}
