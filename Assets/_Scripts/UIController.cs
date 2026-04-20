using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class UIController : MonoBehaviour
{
    public static event Action OnStartGameButtonPressed;
    public static event Action OnExitButtonReleased;
    public static event Action OnResetButtonPressed;
    public static event Action OnZone2ButtonPressed;

    public static event Action OnSceneTransitionRequested;
    public static event Action OnSceneReloadRequested;

    [Header("References")]
    public PlayerStats PlayerStatsData;
    [SerializeField] private UIDocument _uIDocument;

    // Panels
    private VisualElement _mainMenuPanel;
    private VisualElement _transitionPanel;
    private VisualElement _HUDPanel;

    // health
    private VisualElement _healthContainer;
    private VisualElement _healthBlockContainer;
    private VisualElement _healthBlock;

    // ammo
    private VisualElement _ammoContainer;
    private VisualElement _ammoBlockContainer;


    private int _currentHealth;
    private int _maxHealth;
    private int _maxHealthLimit;

    private int _currentAmmo;
    private int _maxAmmo;
    private int _maxAmmoLimit;

    private int _currentCurrency;

    private Button _StartGameButton;
    private Button _ExitGameButton;
    private Button _resetButton;
    private Button _zone2Button;

    [Header("Scene Transition")]
    public float TransitionLength = 1f;
    public float TransitionHoldLength = 2f;
    private Translate _transitionStartPosition;

    private void Awake()
    {
        _healthContainer = _uIDocument.rootVisualElement.Q<VisualElement>("HealthContainer");
        _healthBlockContainer = _healthContainer.Q<VisualElement>("HealthBlockContainer");
        _healthBlock = _healthBlockContainer.Q<VisualElement>("HealthBlock");

        _ammoContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoContainer");
        _ammoBlockContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoBlockContainer");

        _StartGameButton = _uIDocument.rootVisualElement.Q<Button>("StartButton");
        _ExitGameButton = _uIDocument.rootVisualElement.Q<Button>("ExitButton");
        _resetButton = _uIDocument.rootVisualElement.Q<Button>("ResetButton");
        _zone2Button = _uIDocument.rootVisualElement.Q<Button>("Zone2Button");

        _mainMenuPanel = _uIDocument.rootVisualElement.Q<VisualElement>("MainMenu");
        _transitionPanel = _uIDocument.rootVisualElement.Q<VisualElement>("Transition");
        _HUDPanel = _uIDocument.rootVisualElement.Q<VisualElement>("HUD");
    }

    private void OnEnable()
    {
        PlayerHealth.OnMaxHealthChanged += PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged += PlayerHealth_OnCurrentHealthChanged;

        Jump.OnMaxAirJumpsChanged += Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged += Jump_OnCurrentAirJumpAmountChanged;

        CollectStar.OnCurrencyCollected += CollectStar_OnCurrencyCollected;

        _StartGameButton.clicked += StartGameButton_Clicked;
        _ExitGameButton.clicked += ExitGameButton_Clicked;
        _resetButton.clicked += ResetButton_clicked;
        _zone2Button.clicked += Zone2Button_clicked;

        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void OnDisable()
    {
        PlayerHealth.OnMaxHealthChanged -= PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged -= PlayerHealth_OnCurrentHealthChanged;

        Jump.OnMaxAirJumpsChanged -= Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged -= Jump_OnCurrentAirJumpAmountChanged;

        CollectStar.OnCurrencyCollected -= CollectStar_OnCurrencyCollected;

        _StartGameButton.clicked -= StartGameButton_Clicked;
        _ExitGameButton.clicked -= ExitGameButton_Clicked;
        _resetButton.clicked -= ResetButton_clicked;
        _zone2Button.clicked -= Zone2Button_clicked;

        LevelEnd.OnPlayerEnterLevelEnd -= LevelEnd_OnPlayerEnterLevelEnd;
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

    private void StartGameButton_Clicked()
    {
        OnStartGameButtonPressed?.Invoke();
        StartCoroutine(UITransition(_mainMenuPanel, _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Start Game button clicked");
    }

    private void ExitGameButton_Clicked()
    {
        OnExitButtonReleased?.Invoke();
        Debug.Log("Exit Game button clicked, event invoked.");
    }

    private void ResetButton_clicked()
    {
        OnResetButtonPressed?.Invoke();
        StartCoroutine(SceneReload(TransitionLength, TransitionHoldLength));
        Debug.Log("Reset button clicked, event invoked.");
    }

    private void Zone2Button_clicked()
    {
        OnZone2ButtonPressed?.Invoke();
        Debug.Log("Zone 2 button clicked, event invoked.");
    }

    private void LevelEnd_OnPlayerEnterLevelEnd(int sceneIndex)
    {
        StartCoroutine(SceneTransition(_HUDPanel, _HUDPanel, TransitionLength, TransitionHoldLength));
    }

    private IEnumerator UITransition(VisualElement fromElement, VisualElement toElement, float transitionLength, float holdLength)
    {
        // set the transition panel size to be the same as the screen size
        _transitionPanel.style.width = Screen.width;
        _transitionPanel.style.height = Screen.height;

        _transitionPanel.style.translate = new Translate(-Screen.width, 0, 0);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(transitionLength) }
        };

        _transitionPanel.style.translate = new Translate(0, 0, 0);
        yield return new WaitForSeconds(transitionLength);
        fromElement.style.display = DisplayStyle.None;
        toElement.style.display = DisplayStyle.Flex;
        yield return new WaitForSeconds(holdLength);
        _transitionPanel.style.translate = new Translate(Screen.width, 0, 0);
        yield return new WaitForSeconds(transitionLength);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(0) }
        };
        _transitionPanel.style.translate = new Translate(-Screen.width, 0, 0);

    }

    private IEnumerator SceneTransition(VisualElement fromElement, VisualElement toElement, float transitionLength, float holdLength)
    {
        // set the transition panel size to be the same as the screen size
        _transitionPanel.style.width = Screen.width;
        _transitionPanel.style.height = Screen.height;

        _transitionPanel.style.translate = new Translate(-Screen.width, 0, 0);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(transitionLength) }
        };

        _transitionPanel.style.translate = new Translate(0, 0, 0);
        yield return new WaitForSeconds(transitionLength);
        fromElement.style.display = DisplayStyle.None;
        toElement.style.display = DisplayStyle.Flex;
        OnSceneTransitionRequested?.Invoke();
        yield return new WaitForSeconds(holdLength);
        _transitionPanel.style.translate = new Translate(Screen.width, 0, 0);
        yield return new WaitForSeconds(transitionLength);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(0) }
        };
        _transitionPanel.style.translate = new Translate(-Screen.width, 0, 0);

    }

    private IEnumerator SceneReload(float transitionLength, float holdLength)
    {
        // set the transition panel size to be the same as the screen size
        _transitionPanel.style.width = Screen.width;
        _transitionPanel.style.height = Screen.height;

        _transitionPanel.style.translate = new Translate(-Screen.width, 0, 0);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(transitionLength) }
        };

        _transitionPanel.style.translate = new Translate(0, 0, 0);
        yield return new WaitForSeconds(transitionLength);
        OnSceneReloadRequested?.Invoke();
        yield return new WaitForSeconds(holdLength);
        _transitionPanel.style.translate = new Translate(Screen.width, 0, 0);
        yield return new WaitForSeconds(transitionLength);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(0) }
        };
        _transitionPanel.style.translate = new Translate(-Screen.width, 0, 0);

    }

}
