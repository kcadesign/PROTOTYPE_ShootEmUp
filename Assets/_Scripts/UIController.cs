using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class UIController : MonoBehaviour
{
    public static event Action OnResetButtonClicked;
    public static event Action OnZone2ButtonClicked;

    public PlayerStats PlayerStatsData;

    [SerializeField] private UIDocument _uIDocument;
    private VisualElement _healthContainer;
    private VisualElement _healthBlockContainer;
    private VisualElement _healthBlock;


    private VisualElement _ammoContainer;
    private VisualElement _ammoBlockContainer;

    private int _currentHealth;
    private int _maxHealth;
    private int _maxHealthLimit;

    private int _currentAmmo;
    private int _maxAmmo;
    private int _maxAmmoLimit;

    private int _currentCurrency;

    private Button _resetButton;
    private Button _zone2Button;

    private void Awake()
    {
        _healthContainer = _uIDocument.rootVisualElement.Q<VisualElement>("HealthContainer");
        _healthBlockContainer = _healthContainer.Q<VisualElement>("HealthBlockContainer");
        _healthBlock = _healthBlockContainer.Q<VisualElement>("HealthBlock");

        _ammoContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoContainer");
        _ammoBlockContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoBlockContainer");

        _resetButton = _uIDocument.rootVisualElement.Q<Button>("ResetButton");
        _zone2Button = _uIDocument.rootVisualElement.Q<Button>("Zone2Button");
    }

    private void OnEnable()
    {
        PlayerHealth.OnMaxHealthChanged += PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged += PlayerHealth_OnCurrentHealthChanged;

        Jump.OnMaxAirJumpsChanged += Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged += Jump_OnCurrentAirJumpAmountChanged;

        CollectStar.OnCurrencyCollected += CollectStar_OnCurrencyCollected;

        _resetButton.clicked += ResetButton_clicked;
        _zone2Button.clicked += Zone2Button_clicked;
    }

    private void OnDisable()
    {
        PlayerHealth.OnMaxHealthChanged -= PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged -= PlayerHealth_OnCurrentHealthChanged;

        Jump.OnMaxAirJumpsChanged -= Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged -= Jump_OnCurrentAirJumpAmountChanged;

        CollectStar.OnCurrencyCollected -= CollectStar_OnCurrencyCollected;

        _resetButton.clicked -= ResetButton_clicked;
        _zone2Button.clicked -= Zone2Button_clicked;
    }

    private void Start()
    {
        _maxHealthLimit = _healthContainer.childCount;
        _maxAmmoLimit = _healthContainer.childCount;
        PlayerStatsData.SetCurrentCurrency(0);
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

    private void Jump_OnMaxAirJumpsChanged(int maxAirJumps)
    {
        UpdateMaxAmmo(maxAirJumps);
    }

    private void Jump_OnCurrentAirJumpAmountChanged(int currentAirJumpAmount)
    {
        UpdateCurrentAmmo(currentAirJumpAmount);
    }

    private void CollectStar_OnCurrencyCollected(int amount)
    {
        _currentCurrency += amount;
        //Debug.Log($"Current currency: {_currentCurrency}");
        PlayerStatsData.SetCurrentCurrency(_currentCurrency);
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

    private void UpdateMaxAmmo(int maxAmmo)
    {
        for (int i = 0; i < _ammoContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < maxAmmo)
            {
                _ammoContainer[i].style.display = DisplayStyle.Flex;
            }
            else
            {
                _ammoContainer[i].style.display = DisplayStyle.None;
            }
        }
    }

    private void UpdateCurrentAmmo(int currentAmmo)
    {
        for (int i = 0; i < _ammoContainer.childCount; i++)
        {
            // set the number of children that display:flex to be the same as the max health, set the rest as none
            if (i < currentAmmo)
            {
                // get the child of _healthContainer[i] and set it to visible
                _ammoContainer[i][0].visible = true;
            }
            else
            {
                // get the child of _healthContainer[i] and set it to not visible
                _ammoContainer[i][0].visible = false;
            }
        }
    }

    private void ResetButton_clicked()
    {
        OnResetButtonClicked?.Invoke();
        Debug.Log("Reset button clicked, event invoked.");
    }

    private void Zone2Button_clicked()
    {
        OnZone2ButtonClicked?.Invoke();
        Debug.Log("Zone 2 button clicked, event invoked.");
    }
}
