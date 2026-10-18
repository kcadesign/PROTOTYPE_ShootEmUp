using UnityEngine;
using UnityEngine.EventSystems;

public class Chase : MonoBehaviour
{
    private GameObject _player;
    public float DistanceOffset = -3f;
    public float FollowSpeed = 2f;
    public float CreepSpeed = 1f;

    private bool _isBossFight = false;

    //private Vector3 _startPosition;

    private HandleGameState.GameState _currentGameState;

    private void Awake()
    {
        // find the player in the scene by searching for the "Player" tag
        FindPlayer();
        //_startPosition = transform.position;
        DontDestroyOnLoad(gameObject);
    }

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
        _currentGameState = state;
    }

    private void SignalBossLevel_OnExitBossLevel()
    {
        _isBossFight = false;
    }

    private void FixedUpdate()
    {
        //Debug.Log("Boss Fight Status: " + _isBossFight);

        switch (_currentGameState)
        {
            case HandleGameState.GameState.PreGameMenu:
                break;
            case HandleGameState.GameState.Transition:
                break;
            case HandleGameState.GameState.Gameplay:
                if (!_isBossFight)
                {
                    if (_player != null)
                    {
                        MoveTowardsPlayer(FollowSpeed);
                    }
                    else if (_player == null)
                    {
                        FindPlayer();
                    }
                    Creep(CreepSpeed);
                }
                else if (_isBossFight)
                {
                    if (_player != null)
                    {
                        PauseChaserMovement();
                    }
                    else if (_player == null)
                    {
                        FindPlayer();
                    }
                }
                break;
            case HandleGameState.GameState.LevelStart:
                break;
            case HandleGameState.GameState.GamePaused:
                break;
            case HandleGameState.GameState.Shop:
                break;
            case HandleGameState.GameState.LevelEnd:
                MoveTowardsPlayer(0);
                Creep(0);
                break;
            case HandleGameState.GameState.ChoosePowerup:
                break;
            case HandleGameState.GameState.BossFight:
                _isBossFight = true;
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

    private void MoveTowardsPlayer(float followSpeed)
    {
        float desiredY = _player.transform.position.y + DistanceOffset;
        // Prevent downward movement: choose the higher of current Y position and desired Y position
        float targetY = Mathf.Max(transform.position.y, desiredY);

        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
    }

    private void Creep(float creepSpeed)
    {
        gameObject.transform.Translate(Vector2.up * creepSpeed * Time.deltaTime);
    }

    public void PauseChaserMovement()
    {
        MoveTowardsPlayer(0);
        Creep(0);
    }

    public void SetBossFight(bool isBossFight)
    {
        _isBossFight = isBossFight;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }
}
