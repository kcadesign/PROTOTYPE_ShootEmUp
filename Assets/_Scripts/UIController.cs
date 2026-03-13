using System;
using UnityEngine;
using UnityEngine.UIElements;


public class UIController : MonoBehaviour
{
    public static event Action OnResetButtonClicked;

    [SerializeField] private UIDocument _uIDocument;
    private VisualElement _healthContainer;
    private VisualElement _healthBlockContainer;
    private VisualElement _healthBlock;

    private int _currentHealth;
    private int _maxHealth;
    private int _maxHealthLimit;

    private Button _resetButton;

    private void Awake()
    {
        _healthContainer = _uIDocument.rootVisualElement.Q<VisualElement>("HealthContainer");
        _healthBlockContainer = _healthContainer.Q<VisualElement>("HealthBlockContainer");
        _healthBlock = _healthBlockContainer.Q<VisualElement>("HealthBlock");

        _resetButton = _uIDocument.rootVisualElement.Q<Button>("ResetButton");
    }

    private void OnEnable()
    {
        PlayerHealth.OnMaxHealthChanged += PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged += PlayerHealth_OnCurrentHealthChanged;

        _resetButton.clicked += ResetButton_clicked;
    }

    private void OnDisable()
    {
        PlayerHealth.OnMaxHealthChanged -= PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged -= PlayerHealth_OnCurrentHealthChanged;

        _resetButton.clicked -= ResetButton_clicked;
    }

    private void Start()
    {
        _maxHealthLimit = _healthContainer.childCount;
    }

    private void PlayerHealth_OnMaxHealthChanged(int maxHealth)
    {
        Debug.Log($"Max health changed to {maxHealth}");
        //_maxHealth = maxHealth;
        UpdateMaxHealth(maxHealth);
    }

    private void PlayerHealth_OnCurrentHealthChanged(int currentHealth)
    {
        //_currentHealth = currentHealth;
        UpdateCurrentHealth(currentHealth);
    }

    private void UpdateMaxHealth(int maxHealth)
    {
        //Debug.Log($"Health container has {_healthContainer.childCount} children");
        for (int i = 0; i < _healthContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < maxHealth)
            {
                _healthContainer[i].style.display = DisplayStyle.Flex;
            }
            else
            {
                _healthContainer[i].style.display = DisplayStyle.None;
            }
        }
    }

    private void UpdateCurrentHealth(int currentHealth)
    {
        // there is one health block inside each health block container
        // set the number of health blocks that are visible to be the same as the current health, set the rest as not visible
        for (int i = 0; i < _healthContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < currentHealth)
            {
                // get the child of _healthContainer[i] and set it to visible
                _healthContainer[i][0].visible = true;
            }
            else
            {
                // get the child of _healthContainer[i] and set it to not visible
                _healthContainer[i][0].visible = false;
            }
        }

    }

    private void ResetButton_clicked()
    {
        OnResetButtonClicked?.Invoke();
        Debug.Log("Reset button clicked, event invoked.");
    }
}
