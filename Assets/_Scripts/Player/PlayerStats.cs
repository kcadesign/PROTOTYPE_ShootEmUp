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
    public void SetCurrentCurrency(int currentCurrency)
    {
        _currentCurrency = currentCurrency;
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
        if (PlayerPrefs.HasKey("AllowAirJumps"))_allowAirJump = PlayerPrefs.GetInt("AllowAirJumps") == 1;
        if (PlayerPrefs.HasKey("MaxAirJumps"))_maxHealth = PlayerPrefs.GetInt("MaxAirJumps");
        if (PlayerPrefs.HasKey("MaxHealth"))_maxHealth = PlayerPrefs.GetInt("MaxHealth");
    }
}
