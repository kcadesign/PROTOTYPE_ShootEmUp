using UnityEngine;
using UnityEngine.EventSystems;

public class Chase : MonoBehaviour
{
    private GameObject _player;
    public float DistanceOffset = -3f;
    public float FollowSpeed = 2f;
    public float CreepSpeed = 1f;

    private Vector3 _initialPosition;

    private HandleGameState.GameState _currentGameState;

    private void Awake()
    {
        // find the player in the scene by searching for the "Player" tag
        FindPlayer();
        _initialPosition = transform.position;
        DontDestroyOnLoad(gameObject);
    }

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
        _currentGameState = state;

    }

    private void FixedUpdate()
    {
        switch (_currentGameState)
        {
            case HandleGameState.GameState.PreGameMenu:
                break;
            case HandleGameState.GameState.Transition:
                break;
            case HandleGameState.GameState.Gameplay:
                if (_player != null)
                {
                    MoveTowardsPlayer();
                }
                else if (_player == null)
                {
                    FindPlayer();
                }
                Creep();
                break;
            case HandleGameState.GameState.LevelStart:
                transform.position = _initialPosition; // Reset position to initial position
                break;
            case HandleGameState.GameState.GamePaused:
                break;
            case HandleGameState.GameState.Shop:
                break;
            case HandleGameState.GameState.LevelEnd:
                break;
            case HandleGameState.GameState.ChoosePowerup:
                break;
            case HandleGameState.GameState.BossFight:
                break;
            case HandleGameState.GameState.RunEnd:
                break;
            case HandleGameState.GameState.XPTally:
                break;
            case HandleGameState.GameState.GameRestart:
                break;
            case HandleGameState.GameState.GameFinished:
                break;
            case HandleGameState.GameState.Credits:
                break;
            default:
                break;
        }

    }

    private void FindPlayer()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        if (_player == null)
        {
            Debug.LogWarning("Player not found. Make sure the player GameObject has the tag 'Player'.");
        }
    }

    private void MoveTowardsPlayer()
    {
        float desiredY = _player.transform.position.y + DistanceOffset;
        // Prevent downward movement: choose the higher of current Y position and desired Y position
        float targetY = Mathf.Max(transform.position.y, desiredY);

        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * FollowSpeed);
    }

    private void Creep()
    {
        gameObject.transform.Translate(Vector2.up * CreepSpeed * Time.deltaTime);
    }
}
