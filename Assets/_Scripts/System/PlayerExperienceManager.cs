using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;


public class PlayerExperienceManager : MonoBehaviour
{
    public UIDocument UIDocument;
    public PlayerStats PlayerStatsData;

    [Header("Experience Settings")]
    public AnimationCurve ExperienceCurve;

    private int _currentLevel;
    private int _totalXP;
    private int _previousLevelXP;
    private int _nextLevelXP;

    private float _experienceBarFillAmount;
    private float _experienceBarCurrentFillAmount;
    public float XPBarFillSpeed = 0.5f;

    private void Awake()
    {
        //PlayerStatsData.ResetCurrentXP();
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
        //PlayerStatsData.AddToCurrentXP(amount);
        PlayerStatsData.AddToTotalXP(amount);
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
                //PlayerStatsData.ResetCurrentXP();
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
        // Ensure local level/thresholds match PlayerStatsData at startup
        _currentLevel = PlayerStatsData.GetPlayerLevel();
        UpdateLevel();
    }

    private void Update()
    {
        //if(_experienceBarCurrentFillAmount < _experienceBarFillAmount)
        //{
        //    _experienceBarCurrentFillAmount = Mathf.MoveTowards(_experienceBarCurrentFillAmount, _experienceBarFillAmount, Time.deltaTime * XPBarFillSpeed);
        //    PlayerStatsData.SetCurrentXPValue(_experienceBarCurrentFillAmount);
        //}

    }

    public void AddExperience()
    {
        _totalXP = PlayerStatsData.GetTotalXP();
        CheckForLevelUp();
        UpdateInterface();
    }

    private void CheckForLevelUp()
    {
        // Make sure thresholds are initialized before checking
        if (_nextLevelXP <= 0)
            UpdateLevel();

        while (_nextLevelXP > 0 && _totalXP >= _nextLevelXP)
        {
            PlayerStatsData.IncreasePlayerLevel();
            _currentLevel = PlayerStatsData.GetPlayerLevel();
            UpdateLevel();
        }
    }

    private void UpdateLevel()
    {
        _previousLevelXP = (int)ExperienceCurve.Evaluate(_currentLevel);
        _nextLevelXP = (int)ExperienceCurve.Evaluate(_currentLevel + 1);
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        int lowValue = _totalXP - _previousLevelXP;
        int highValue = _nextLevelXP - _previousLevelXP;

        // Safety: avoid negative/zero range
        if (highValue <= 0) highValue = 1;
        lowValue = Mathf.Clamp(lowValue, 0, highValue);

        PlayerStatsData.SetLowXPValue(lowValue);
        PlayerStatsData.SetHighXPValue(highValue);

        _experienceBarFillAmount = (float)lowValue / (float)highValue;
        // move towards target fill amount smoothly
        StartCoroutine(XPBarFillCoroutine(_experienceBarFillAmount));
    }

    private IEnumerator XPBarFillCoroutine(float targetFillAmount)
    {
        while (_experienceBarCurrentFillAmount < targetFillAmount)
        {
            _experienceBarCurrentFillAmount = Mathf.MoveTowards(_experienceBarCurrentFillAmount, targetFillAmount, Time.deltaTime * XPBarFillSpeed);
            PlayerStatsData.SetCurrentXPValue(_experienceBarCurrentFillAmount);
            yield return null;
        }
        _experienceBarCurrentFillAmount = targetFillAmount;
    }

    private IEnumerator DelayXPTally(float delay)
    {
        yield return new WaitForSeconds(delay);
        AddExperience();
    }
}
