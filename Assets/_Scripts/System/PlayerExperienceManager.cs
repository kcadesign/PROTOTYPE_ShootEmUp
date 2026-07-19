using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerExperienceManager : MonoBehaviour
{
    public static event Action<int> OnPlayerLevelUp;

    public UIDocument UIDocument;
    public PlayerStats PlayerStatsData;

    [Header("Experience Settings")]
    //public AnimationCurve ExperienceCurve;

    //private int _currentLevel;
    //private int _totalXP;
    //private int _previousLevelXP;
    //private int _nextLevelXP;

    //private float _XPBarTargetFillAmount;
    //private float _XPBarCurrentFillAmount;
    public float XPBarFillSpeed = 0.5f;

    private int _currentLevel;
    private int _runExp;
    private int _storedExp;
    private int _totalExp;
    private float _barValue = 0;
    private int _expToLevel = 15;
    private int _levelUpRemainder = 0;
    public float ExpToLevelGrowth = 1.15f;

    private void Awake()
    {
        //PlayerStatsData.ResetCurrentXP();
        Debug.Log($"Player Experience Manager: OnEnable - Current Level: {_currentLevel}, Total Exp: {_totalExp}, Stored Exp: {_storedExp}, Run Exp: {_runExp}");
        Debug.Log($"Player Stats Data: OnEnableCurrent Level: {PlayerStatsData.GetPlayerLevel()}, Total Exp: {PlayerStatsData.GetTotalExp()}, Stored Exp: {PlayerStatsData.GetStoredExp()}, Run Exp: {PlayerStatsData.GetRunExp()}");
    }

    private void OnEnable()
    {
        HandleDeath.OnEnemyDeath += HandleDeath_OnEnemyDeath;
        CollectXP.OnXPCollected += CollectXP_OnXPCollected;

        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;
    }

    private void OnDisable()
    {
        HandleDeath.OnEnemyDeath -= HandleDeath_OnEnemyDeath;
        CollectXP.OnXPCollected -= CollectXP_OnXPCollected;

        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;

        PlayerStatsData.ResetRunEnemiesKilled();
        //PlayerStatsData.ResetCurrentXP();
    }

    private void HandleDeath_OnEnemyDeath()
    {
        PlayerStatsData.AddEnemiesKilled();
    }

    private void CollectXP_OnXPCollected(int amount)
    {
        PlayerStatsData.IncreaseTotalExp(amount);
        PlayerStatsData.IncreaseRunExp(amount);
        _totalExp = PlayerStatsData.GetTotalExp();
        _runExp = PlayerStatsData.GetRunExp();
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        switch (state)
        {
            case HandleGameState.GameState.PreGameMenu:
                break;
            case HandleGameState.GameState.Transition:
                break;
            case HandleGameState.GameState.LevelStart:
                break;
            case HandleGameState.GameState.Gameplay:
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
                StartCoroutine(DelayXPTally(3f));
                break;
            case HandleGameState.GameState.GameRestart:
                ResetRunValues();
                break;
            case HandleGameState.GameState.GameFinished:
                break;
            case HandleGameState.GameState.Credits:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    //private void Start()
    //{
    //    // Ensure local level/thresholds match PlayerStatsData at startup
    //    _currentLevel = PlayerStatsData.GetPlayerLevel();
    //    UpdateLevel();
    //}

    //public void AddExperience()
    //{
    //    _totalXP = PlayerStatsData.GetTotalXP();
    //    CheckForLevelUp();
    //    UpdateInterface();
    //}

    //private void CheckForLevelUp()
    //{
    //    // Make sure thresholds are initialized before checking
    //    if (_nextLevelXP <= 0)
    //        UpdateLevel();

    //    while (_nextLevelXP > 0 && _totalXP >= _nextLevelXP)
    //    {
    //        PlayerStatsData.IncreasePlayerLevel();
    //        _currentLevel = PlayerStatsData.GetPlayerLevel();
    //        UpdateLevel();
    //    }
    //}

    //private void UpdateLevel()
    //{
    //    _previousLevelXP = (int)ExperienceCurve.Evaluate(_currentLevel);
    //    _nextLevelXP = (int)ExperienceCurve.Evaluate(_currentLevel + 1);
    //    UpdateInterface();
    //}

    //private void UpdateInterface()
    //{
    //    int lowValue = _totalXP - _previousLevelXP;
    //    int highValue = _nextLevelXP - _previousLevelXP;

    //    // Safety: avoid negative/zero range
    //    if (highValue <= 0) highValue = 1;
    //    lowValue = Mathf.Clamp(lowValue, 0, highValue);

    //    PlayerStatsData.SetLowXPValue(lowValue);
    //    PlayerStatsData.SetHighXPValue(highValue);

    //    _XPBarTargetFillAmount = (float)lowValue / (float)highValue;
    //    Debug.Log(_XPBarTargetFillAmount);

    //    // move towards target fill amount smoothly
    //    StartCoroutine(XPBarFillCoroutine(_XPBarTargetFillAmount));
    //}

    //private IEnumerator XPBarFillCoroutine(float targetFillAmount)
    //{
    //    while (_XPBarCurrentFillAmount < targetFillAmount)
    //    {
    //        _XPBarCurrentFillAmount = Mathf.MoveTowards(_XPBarCurrentFillAmount, targetFillAmount, Time.deltaTime * XPBarFillSpeed);
    //        PlayerStatsData.SetCurrentXPValue(_XPBarCurrentFillAmount);
    //        yield return null;
    //    }
    //    _XPBarCurrentFillAmount = targetFillAmount;

    //}

    //private IEnumerator DelayXPTally(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    AddExperience();
    //}

    private void Start()
    {
        _storedExp = PlayerStatsData.GetStoredExp();
    }

    private IEnumerator DelayXPTally(float delay)
    {
        PlayerStatsData.SetLowExpValue(0);
        PlayerStatsData.SetHighExpValue(_expToLevel);

        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(TallyExperience());
    }

    public IEnumerator TallyExperience()
    {
        PlayerStatsData.SetStoredExp(_storedExp);
        _barValue = PlayerStatsData.GetExpBarFillValue();

        if ((_storedExp + _runExp) >= _expToLevel)
        {
            // Move bar fill towards maximum
            while (_barValue < _expToLevel)
            {
                float fillValue = Mathf.MoveTowards(_barValue, _expToLevel, Time.deltaTime * XPBarFillSpeed);
                _barValue = fillValue;
                PlayerStatsData.SetExpBarFillValue(fillValue);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            _barValue = 0;
            PlayerStatsData.SetExpBarFillValue(_barValue);

            LevelUp();

            _levelUpRemainder = (_storedExp + _runExp) - _expToLevel;
            Debug.Log($"Level Up! Remainder: {_levelUpRemainder}");

            _expToLevel = Mathf.RoundToInt(_expToLevel * ExpToLevelGrowth);
            PlayerStatsData.SetHighExpValue(_expToLevel);

            while (_barValue < _levelUpRemainder)
            {
                float fillValue = Mathf.MoveTowards(_barValue, _levelUpRemainder, Time.deltaTime * XPBarFillSpeed);
                _barValue = fillValue;
                PlayerStatsData.SetExpBarFillValue(fillValue);
                yield return null;
            }
            _storedExp = _levelUpRemainder;
            _levelUpRemainder = 0;
        }
        else if ((_storedExp + _runExp) < _expToLevel)
        {
            while (_barValue < (_storedExp + _runExp))
            {
                float fillValue = Mathf.MoveTowards(_barValue, (_storedExp + _runExp), Time.deltaTime * XPBarFillSpeed);
                _barValue = fillValue;
                PlayerStatsData.SetExpBarFillValue(fillValue);
                yield return null;
            }
        }
        StoreRunExp();
        PlayerStatsData.SetStoredExp(_storedExp);
        ResetRunExp();
        PlayerStatsData.ResetRunExp();
    }

    private void ResetRunExp()
    {
        _runExp = 0;
    }

    private void StoreRunExp()
    {
        _storedExp += _runExp;
    }

    private IEnumerator AnimateBarFill(float fromvalue, float toValue)
    {
        while (fromvalue < toValue)
        {
            float fillValue = Mathf.MoveTowards(fromvalue, toValue, Time.deltaTime * XPBarFillSpeed);
            PlayerStatsData.SetExpBarFillValue(fillValue);
            yield return null;
        }
    }

    private void LevelUp()
    {
        PlayerStatsData.IncreasePlayerLevel();
        OnPlayerLevelUp?.Invoke(PlayerStatsData.GetPlayerLevel());
    }

    private void IncrementNextLevelExp()
    {
        _expToLevel = Mathf.RoundToInt(_expToLevel * ExpToLevelGrowth);
    }

    private void UpdateUINextLevel()
    {
        PlayerStatsData.SetHighExpValue(_expToLevel);
    }

    private void ResetRunValues()
    {
        _runExp = 0;
        PlayerStatsData.ResetRunExp();
        PlayerStatsData.ResetRunEnemiesKilled();
    }
}
