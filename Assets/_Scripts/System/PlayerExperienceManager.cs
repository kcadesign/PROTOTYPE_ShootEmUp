using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerExperienceManager : MonoBehaviour
{
    public static event Action<int> OnPlayerLevelUp;
    public static event Action OnExpTallied;

    public UIDocument UIDocument;
    public PlayerStats PlayerStatsData;

    [Header("Experience Settings")]
    public float XPBarFillSpeed = 0.5f;

    private int _currentLevel;
    private int _runExp;
    private int _storedExp;
    private int _totalExp;
    private float _barValue = 0;
    private int _expToLevel = 40;
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

        // Calculate the total XP available, including XP from this run.
        int remainingExp = _storedExp + _runExp;

        // Keep levelling up while enough XP remains.
        while (remainingExp >= _expToLevel)
        {
            // Animate the bar to the current level's XP requirement.
            while (_barValue < _expToLevel)
            {
                _barValue = Mathf.MoveTowards(_barValue, _expToLevel, Time.deltaTime * XPBarFillSpeed);

                PlayerStatsData.SetExpBarFillValue(_barValue);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            // Subtract the XP required for this level.
            remainingExp -= _expToLevel;

            // Reset the bar for the next level.
            _barValue = 0;
            PlayerStatsData.SetExpBarFillValue(_barValue);

            // Apply the level-up.
            LevelUp();

            // Increase the XP requirement for the next level.
            _expToLevel = Mathf.RoundToInt(_expToLevel * ExpToLevelGrowth);
            PlayerStatsData.SetHighExpValue(_expToLevel);
        }

        // Animate any remaining XP into the bar.
        while (_barValue < remainingExp)
        {
            _barValue = Mathf.MoveTowards(_barValue, remainingExp, Time.deltaTime * XPBarFillSpeed);

            PlayerStatsData.SetExpBarFillValue(_barValue);
            yield return null;
        }

        // Save the remaining XP for the next run.
        _storedExp = remainingExp;

        PlayerStatsData.SetStoredExp(_storedExp);

        ResetRunExp();
        PlayerStatsData.ResetRunExp();

        OnExpTallied?.Invoke();
    }

    private void ResetRunExp()
    {
        _runExp = 0;
    }

    private void StoreRunExp()
    {
        _storedExp += _runExp;
    }

    private void ResetStoredExp()
    {
        _storedExp = 0;
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
