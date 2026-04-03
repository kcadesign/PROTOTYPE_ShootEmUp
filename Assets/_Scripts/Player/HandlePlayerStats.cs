using UnityEngine;

public class HandlePlayerStats : MonoBehaviour
{
    public PlayerStats PlayerStatsData;

    private Jump _playerJump;
    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _playerJump = GetComponent<Jump>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        PlayerStatsData.LoadAllDataFromPrefs();

        _playerJump.SetAllowAirJumps(PlayerStatsData.GetCanAirJump());
        _playerJump.SetMaxAirJumps(PlayerStatsData.GetMaxAirJumps());
        _playerHealth.SetMaxHealth(PlayerStatsData.GetMaxHealth());

        PlayerStatsData.SetMaxHealth(_playerHealth.MaxHealth);
        PlayerStatsData.SetCanAirJump(_playerJump.AllowAirJumps);
        PlayerStatsData.SetMaxAirJumps(_playerJump.MaxAirJumps);
    }

    private void OnEnable()
    {
        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_PlayerEnteredLevelEnd;
    }

    private void OnDisable()
    {
        LevelEnd.OnPlayerEnterLevelEnd -= LevelEnd_PlayerEnteredLevelEnd;
        PlayerStatsData.SaveAllDataToPrefs();
    }

    private void LevelEnd_PlayerEnteredLevelEnd(int levelIndex)
    {
        PlayerStatsData.SetMaxHealth(_playerHealth.MaxHealth);
        PlayerStatsData.SetCanAirJump(_playerJump.AllowAirJumps);
        PlayerStatsData.SetMaxAirJumps(_playerJump.MaxAirJumps);
        PlayerStatsData.SaveAllDataToPrefs();
    }
}
