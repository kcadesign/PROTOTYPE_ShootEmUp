using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Scriptable Objects/Player/PlayerStatsData")]
public class PlayerStats : ScriptableObject
{
    //public Jump _player_jump;
    //public PlayerHealth _player_health;
    [Header("Air Jump")]
    [SerializeField] private bool _allowAirJump = false;
    [SerializeField] private int _maxAirJumps = 0;

    [Header("Health")]
    [SerializeField] private int _maxHealth = 3;

    [Header("Currency")]
    [SerializeField] private int _currentCurrency = 0;
    [SerializeField] private int _runTotalCurrency = 0;
    [SerializeField] private int _lifetimeTotalCurrency = 0;

    [Header("Experience")]
    [SerializeField] private int _playerLevel = 0;
    //[SerializeField] private int _currentXP = 0;
    [SerializeField] private int _totalXP = 0;
    [SerializeField] private int _xPLowValue;
    [SerializeField] private int _xPHighValue;
    [SerializeField] private float _XPBarFillValue;

    [Header("Enemies")]
    [SerializeField] private int _runEnemiesKilled;
    [SerializeField] private int _lifetimeEnemiesKilled;

    [Header("Levels")]
    [SerializeField] private int _runHighestLevel;
    [SerializeField] private int _lifetimeHighestLevel;

    [Header("Time")]
    [SerializeField] private float _runLength;
    [SerializeField] private string _timerAsText;

    public bool GetCanAirJump() { return _allowAirJump; }
    public void SetCanAirJump(bool canAirJump)
    {
        _allowAirJump = canAirJump;
    }

    public int GetMaxAirJumps() { return _maxAirJumps; }
    public void SetMaxAirJumps(int maxAirJumps)
    {
        _maxAirJumps = maxAirJumps;
    }

    public int GetMaxHealth() { return _maxHealth; }
    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }

    public int GetCurrentCurrency() { return _currentCurrency; }
    public void AddToCurrentCurrency(int amount)
    {
        _currentCurrency += amount;
    }
    public void SubtractFromCurrentCurrency(int amount)
    {
        _currentCurrency -= amount;
        if (_currentCurrency < 0) _currentCurrency = 0;
    }
    public void ResetCurrentCurrency()
    {
        _currentCurrency = 0;
    }

    public int GetRunTotalCurrency() { return _runTotalCurrency; }
    public void AddToRunTotalCurrency(int amount)
    {
        _runTotalCurrency += amount;
    }
    public void ResetRunTotalCurrency()
    {
        _runTotalCurrency = 0;
    }

    public int GetLifetimeTotalCurrency() { return _lifetimeTotalCurrency; }
    public void AddToLifetimeTotalCurrency(int amount)
    {
        _lifetimeTotalCurrency += amount;
    }
    public void ResetLifetimeTotalCurrency()
    {
        _lifetimeTotalCurrency = 0;
    }

    public int GetPlayerLevel() { return _playerLevel; }
    public void IncreasePlayerLevel()
    {
        _playerLevel ++;
    }

    //public int GetCurrentXP() { return _currentXP; }
    //public void AddToCurrentXP(int currentXP)
    //{
    //    _currentXP += currentXP;
    //}
    //public void ResetCurrentXP()
    //{
    //    _currentXP = 0;
    //}
    public int GetTotalXP() { return _totalXP; }
    public void AddToTotalXP(int amount)
    {
        _totalXP += amount;
    }

    public void SetRunTime(float runTime)
    {
        _runLength = runTime;
    }
    public void SetTimerAsText(string timerText)
    {
        _timerAsText = timerText;
    }

    public void SaveAllDataToPrefs()
    {
        PlayerPrefs.SetInt("AllowAirJumps", _allowAirJump ? 1 : 0);
        PlayerPrefs.SetInt("MaxAirJumps", _maxAirJumps);
        PlayerPrefs.SetInt("MaxHealth", _maxHealth);
        PlayerPrefs.Save();
    }

    public void LoadAllDataFromPrefs()
    {
        if (PlayerPrefs.HasKey("AllowAirJumps")) _allowAirJump = PlayerPrefs.GetInt("AllowAirJumps") == 1;
        if (PlayerPrefs.HasKey("MaxAirJumps")) _maxHealth = PlayerPrefs.GetInt("MaxAirJumps");
        if (PlayerPrefs.HasKey("MaxHealth")) _maxHealth = PlayerPrefs.GetInt("MaxHealth");
    }

    public void SetLowXPValue(int lowValue)
    {
        _xPLowValue = lowValue;
    }

    public void SetHighXPValue(int highValue)
    {
        _xPHighValue = highValue;
    }

    public void SetCurrentXPValue(float value)
    {
        _XPBarFillValue = value;
    }

    public void AddEnemiesKilled()
    {
        _runEnemiesKilled++;
        _lifetimeEnemiesKilled++;
    }
    public void ResetRunEnemiesKilled()
    {
        _runEnemiesKilled = 0;
    }
}
