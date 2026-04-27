using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerExperienceManager : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction _click;

    public UIDocument UIDocument;
    public PlayerStats PlayerStatsData;

    [Header("Experience Settings")]
    public AnimationCurve ExperienceCurve;

    private int _currentLevel;
    private int _currentXP;
    private int _previousLevelXP;
    private int _nextLevelXP;

    private Label _levelText;
    private Label _XPText;

    private float _experienceBarFillAmount;

    private void Awake()
    {
        _click = InputActions.FindAction("Click");

        _levelText = UIDocument.rootVisualElement.Q<Label>("LevelText");
        _XPText = UIDocument.rootVisualElement.Q<Label>("XPText");
    }

    private void Start()
    {
        UpdateLevel();
    }

    private void Update()
    {
        if(_click.WasPressedThisFrame())
        {
            AddExperience(5);
        }
    }

    public void AddExperience(int amount)
    {
        _currentXP += amount;
        CheckForLevelUp();
        UpdateInterface();
    }

    private void CheckForLevelUp()
    {
        while (_currentXP >= _nextLevelXP)
        {
            _currentLevel++;
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
        int lowValue = _currentXP - _previousLevelXP;
        int highValue = _nextLevelXP - _previousLevelXP;

        PlayerStatsData.SetLowValue(lowValue);
        PlayerStatsData.SetHighValue(highValue);

        _levelText.text = "LEVEL " + _currentLevel.ToString();
        //_XPText.text = lowValue + " STARS / " + highValue + " STARS";
        _experienceBarFillAmount = (float)lowValue / (float)highValue;
        PlayerStatsData.SetValue(_experienceBarFillAmount);
    }
}
