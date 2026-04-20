using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject PlayerPrefab;
    private GameObject _player;
    public Vector3 SpawnPosition;

    private void OnEnable()
    {
        SceneController.OnLevelLoaded += SceneController_OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneController.OnLevelLoaded -= SceneController_OnLevelLoaded;
    }

    private void SceneController_OnLevelLoaded()
    {
        if(_player == null)
        {
            Debug.Log("Player not found. Spawning player at spawn position.");
            InstantiatePlayer();
        }
        else if (_player != null)
        {
            Debug.Log("Player already exists. Moving player to spawn position.");
            _player.transform.position = SpawnPosition;
            
            if (!_player.activeSelf)
            {
                Debug.Log("Player inactive. Setting active now and resetting health.");
                _player.SetActive(true);
                _player.GetComponent<PlayerHealth>()?.ResetHealth(); // Reset health when respawning
            }
        }

    }

    private void InstantiatePlayer()
    {
        _player = Instantiate(PlayerPrefab, SpawnPosition, Quaternion.identity);
        // set the player to not destroy on load so it persists across scenes
        DontDestroyOnLoad(_player);
    }
}
