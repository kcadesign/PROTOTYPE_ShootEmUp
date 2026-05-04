using UnityEngine;

public class Timer : MonoBehaviour
{
    public PlayerStats PlayerStatsData;
    private float _timeElapsed;
    private string _timerAsText;

    private bool _timerActive;

    private void OnEnable()
    {
        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;

        ResetTimer();
    }

    private void OnDisable()
    {
        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;

        ResetTimer();
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState state)
    {
        switch (state)
        {
            case HandleGameState.GameState.PreGameMenu:
                ResetTimer();
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.Transition:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.LevelStart:
                SetTimerActive(true);
                break;
            case HandleGameState.GameState.Gameplay:
                SetTimerActive(true);
                break;
            case HandleGameState.GameState.GamePaused:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.Shop:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.LevelEnd:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.ChoosePowerup:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.BossFight:
                SetTimerActive(true);
                break;
            case HandleGameState.GameState.RunEnd:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.XPTally:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.GameFinished:
                SetTimerActive(false);
                break;
            case HandleGameState.GameState.Credits:
                SetTimerActive(false);
                break;
        }
    }

    private void Update()
    {
        if (_timerActive)
        {
            IncreaseTimeElapsed();
        }
    }

    private void SetTimerActive(bool active)
    {
        _timerActive = active;
    }

    private void IncreaseTimeElapsed()
    {
        _timeElapsed += Time.deltaTime;
        PlayerStatsData.SetRunTime(_timeElapsed);
        UpdateTimerText();
    }

    private void ResetTimer()
    {
        _timeElapsed = 0f;
    }

    void UpdateTimerText()
    {
        // Convert timeRemaining to minutes and seconds
        int minutes = Mathf.FloorToInt(_timeElapsed / 60);
        int seconds = Mathf.FloorToInt(_timeElapsed % 60);

        // Format the timer text (MM:SS)
        _timerAsText = string.Format("{0:00}:{1:00}", minutes, seconds);
        PlayerStatsData.SetTimerAsText(_timerAsText);
    }

}
