using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsData", menuName = "Scriptable Objects/Player/PlayerStatsData")]
public class PlayerStats : ScriptableObject
{
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
    [SerializeField] private int _runExp = 0;
    [SerializeField] private int _storedExp = 0;
    [SerializeField] private int _totalExp = 0;
    [SerializeField] private int _expBarLowValue;
    [SerializeField] private int _expBarHighValue;
    [SerializeField] private float _expBarFillValue;

    [Header("Enemies")]
    [SerializeField] private int _runEnemiesKilled;
    [SerializeField] private int _lifetimeEnemiesKilled;

    [Header("Combo")]
    [SerializeField] private int _comboLevel;
    [SerializeField] private int _runBestComboLevel;
    [SerializeField] private int _lifetimeBestComboLevel;

    [Header("Levels")]
    [SerializeField] private int _runHighestLevel;
    [SerializeField] private int _lifetimeHighestLevel;

    [Header("Time")]
    [SerializeField] private float _runLength;
    [SerializeField] private string _timerAsText;

    #region Air Jump
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
    #endregion

    #region Health
    public int GetMaxHealth() { return _maxHealth; }
    public void SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
    }
    #endregion

    #region Currency
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
    #endregion

    #region Experience
    public int GetPlayerLevel() { return _playerLevel; }
    public void IncreasePlayerLevel()
    {
        _playerLevel++;
    }

    public int GetRunExp() { return _runExp; }
    public void IncreaseRunExp(int amount)
    {
        _runExp += amount;
    }
    public void SetRunExp(int amount)
    {
        _runExp = amount;
    }
    public void ResetRunExp()
    {
        _runExp = 0;
    }

    public int GetStoredExp() { return _storedExp; }
    public void SetStoredExp(int amount)
    {
        _storedExp = amount;
    }
    public void ResetStoredExp()
    {
        _storedExp = 0;
    }

    public int GetTotalExp() { return _totalExp; }
    public void IncreaseTotalExp(int amount)
    {
        _totalExp += amount;
    }


    public void SetLowExpValue(int lowValue)
    {
        _expBarLowValue = lowValue;
    }
    public void SetHighExpValue(int highValue)
    {
        _expBarHighValue = highValue;
    }
    public void SetExpBarFillValue(float value)
    {
        _expBarFillValue = value;
    }
    public float GetExpBarFillValue()
    {
        return _expBarFillValue;
    }
    #endregion

    #region Enemies
    public void AddEnemiesKilled()
    {
        _runEnemiesKilled++;
        _lifetimeEnemiesKilled++;
    }
    public void ResetRunEnemiesKilled()
    {
        _runEnemiesKilled = 0;
    }
    #endregion

    #region Combo
    public int GetComboLevel() { return _comboLevel; }
    public void IncreaseComboLevel()
    {
        _comboLevel++;
    }
    public void ResetComboLevel()
    {
        _comboLevel = 0;
    }

    public int GetRunBestComboLevel() { return _runBestComboLevel; }
    public void IncreaseRunBestComboLevel()
    {
        _runBestComboLevel++;
    }
    public void ResetRunBestComboLevel()
    {
        _runBestComboLevel = 0;
    }

    public int GetLifetimeBestComboLevel() { return _lifetimeBestComboLevel; }
    public void IncreaseLifetimeBestComboLevel()
    {
        _lifetimeBestComboLevel++;
    }
    public void ResetLifetimeBestComboLevel()
    {
        _lifetimeBestComboLevel = 0;
    }
    #endregion

    #region Levels
    public int GetRunHighestLevel() { return _runHighestLevel; }
    public void IncreaseRunHighestLevel()
    {
        _runHighestLevel++;
    }
    public void ResetRunHighestLevel()
    {
        _runHighestLevel = 0;
    }

    public int GetLifetimeHighestLevel() { return _lifetimeHighestLevel; }
    public void SetLifetimeHighestLevel(int level)
    {
        _lifetimeHighestLevel = level;
    }
    public void ResetLifetimeHighestLevel()
    {
        _lifetimeHighestLevel = 0;
    }
    #endregion

    #region Time
    public void SetRunTime(float runTime)
    {
        _runLength = runTime;
    }
    public void SetTimerAsText(string timerText)
    {
        _timerAsText = timerText;
    }
    #endregion

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
}
