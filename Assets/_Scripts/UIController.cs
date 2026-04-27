using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class UIController : MonoBehaviour
{
    public static event Action OnStartGameButtonPressed;
    public static event Action<string> OnLevelSelected;
    public static event Action OnExitButtonPressed;

    public static event Action OnToggleAirJumpPressed;
    public static event Action OnAddAirJumpPressed;
    public static event Action OnResetButtonPressed;
    public static event Action OnZone2ButtonPressed;

    public static event Action OnSceneTransitionRequested;
    public static event Action OnSceneReloadRequested;

    [Header("References")]
    public InputActionAsset InputActions;
    public PlayerStats PlayerStatsData;
    [SerializeField] private UIDocument _uIDocument;

    // Panels
    private VisualElement _mainMenuPanel;
    private VisualElement _levelSelectPanel;
    private VisualElement _transitionPanel;
    private VisualElement _HUDPanel;
    private VisualElement _XPPanel;

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

    // Main menu
    private Button _startGameButton;
    private Button _levelSelectButton;
    private Button _exitGameButton;

    // Level select menu
    private Button _level1Button;
    private Button _level2Button;
    private Button _level3Button;
    private Button _level4Button;
    private Button _level5Button;
    private Button _backButton;

    // HUD
    private Button _toggleAirJumpButton;
    private Button _addAirJumpButton;
    private Button _zone2Button;
    private Button _resetButton;

    [Header("Scene Transition")]
    public float TransitionLength = 1f;
    public float TransitionHoldLength = 2f;
    public Translate _transitionStartPosition = new Translate(0, Screen.height, 0);
    public Translate _transitionMidPosition = new Translate(0, 0, 0);
    public Translate _transitionEndPosition = new Translate(0, -Screen.height, 0);

    private void Awake()
    {
        _healthContainer = _uIDocument.rootVisualElement.Q<VisualElement>("HealthContainer");
        _healthBlockContainer = _healthContainer.Q<VisualElement>("HealthBlockContainer");
        _healthBlock = _healthBlockContainer.Q<VisualElement>("HealthBlock");

        _ammoContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoContainer");
        _ammoBlockContainer = _uIDocument.rootVisualElement.Q<VisualElement>("AmmoBlockContainer");

        _startGameButton = _uIDocument.rootVisualElement.Q<Button>("StartButton");
        _levelSelectButton = _uIDocument.rootVisualElement.Q<Button>("LevelSelectButton");
        _exitGameButton = _uIDocument.rootVisualElement.Q<Button>("ExitButton");

        _level1Button = _uIDocument.rootVisualElement.Q<Button>("Level01");
        _level2Button = _uIDocument.rootVisualElement.Q<Button>("Level02");
        _level3Button = _uIDocument.rootVisualElement.Q<Button>("Level03");
        _level4Button = _uIDocument.rootVisualElement.Q<Button>("Level04");
        _level5Button = _uIDocument.rootVisualElement.Q<Button>("Level05");
        _backButton = _uIDocument.rootVisualElement.Q<Button>("BackButton");

        _toggleAirJumpButton = _uIDocument.rootVisualElement.Q<Button>("ToggleAirJump");
        _addAirJumpButton = _uIDocument.rootVisualElement.Q<Button>("AddAirJump");
        _resetButton = _uIDocument.rootVisualElement.Q<Button>("ResetButton");
        _zone2Button = _uIDocument.rootVisualElement.Q<Button>("Zone2Button");

        _mainMenuPanel = _uIDocument.rootVisualElement.Q<VisualElement>("MainMenu");
        _levelSelectPanel = _uIDocument.rootVisualElement.Q<VisualElement>("LevelSelectMenu");
        _transitionPanel = _uIDocument.rootVisualElement.Q<VisualElement>("Transition");
        _HUDPanel = _uIDocument.rootVisualElement.Q<VisualElement>("HUD");
        _XPPanel = _uIDocument.rootVisualElement.Q<VisualElement>("Experience");
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("UI").Enable();

        PlayerHealth.OnMaxHealthChanged += PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged += PlayerHealth_OnCurrentHealthChanged;
        HandlePlayerDeath.OnPlayerDeath += HandlePlayerDeath_OnPlayerDeath;

        Jump.OnMaxAirJumpsChanged += Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged += Jump_OnCurrentAirJumpAmountChanged;

        CollectStar.OnCurrencyCollected += CollectStar_OnCurrencyCollected;

        _startGameButton.clicked += StartGameButton_Clicked;
        _levelSelectButton.clicked += LevelSelectButton_Clicked;
        _exitGameButton.clicked += ExitGameButton_Clicked;

        _level1Button.clicked += Level1Button_Clicked;
        _level2Button.clicked += Level2Button_Clicked;
        _level3Button.clicked += Level3Button_Clicked;
        _level4Button.clicked += Level4Button_Clicked;
        _level5Button.clicked += Level5Button_Clicked;
        _backButton.clicked += BackButton_Clicked;

        _toggleAirJumpButton.clicked += ToggleAirJumpButton_Clicked;
        _addAirJumpButton.clicked += AddAirJumpButton_Clicked;
        _resetButton.clicked += ResetButton_clicked;
        _zone2Button.clicked += Zone2Button_clicked;

        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("UI").Disable();

        PlayerHealth.OnMaxHealthChanged -= PlayerHealth_OnMaxHealthChanged;
        PlayerHealth.OnCurrentHealthChanged -= PlayerHealth_OnCurrentHealthChanged;
        HandlePlayerDeath.OnPlayerDeath -= HandlePlayerDeath_OnPlayerDeath;

        Jump.OnMaxAirJumpsChanged -= Jump_OnMaxAirJumpsChanged;
        Jump.OnCurrentAirJumpAmountChanged -= Jump_OnCurrentAirJumpAmountChanged;

        CollectStar.OnCurrencyCollected -= CollectStar_OnCurrencyCollected;

        _startGameButton.clicked -= StartGameButton_Clicked;
        _levelSelectButton.clicked -= LevelSelectButton_Clicked;
        _exitGameButton.clicked -= ExitGameButton_Clicked;

        _level1Button.clicked -= Level1Button_Clicked;
        _level2Button.clicked -= Level2Button_Clicked;
        _level3Button.clicked -= Level3Button_Clicked;
        _level4Button.clicked -= Level4Button_Clicked;
        _level5Button.clicked -= Level5Button_Clicked;
        _backButton.clicked -= BackButton_Clicked;

        _toggleAirJumpButton.clicked -= ToggleAirJumpButton_Clicked;
        _addAirJumpButton.clicked -= AddAirJumpButton_Clicked;
        _resetButton.clicked -= ResetButton_clicked;
        _zone2Button.clicked -= Zone2Button_clicked;

        LevelEnd.OnPlayerEnterLevelEnd -= LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void Start()
    {
        _maxHealthLimit = _healthContainer.childCount;
        _maxAmmoLimit = _healthContainer.childCount;
        PlayerStatsData.SetCurrentCurrency(0);
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("StartButton").Focus();
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

    private void HandlePlayerDeath_OnPlayerDeath()
    {
        Debug.Log("Player died. Reloading scene");
        StartCoroutine(SceneReload(TransitionLength, TransitionHoldLength));
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
        StartCoroutine(UITransition(/*_mainMenuPanel,*/ _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Start Game button clicked");
    }

    private void LevelSelectButton_Clicked()
    {
        StartCoroutine(UITransition(/*_mainMenuPanel,*/ _levelSelectPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level Select button clicked");
    }

    private void ExitGameButton_Clicked()
    {
        OnExitButtonPressed?.Invoke();
        Debug.Log("Exit Game button clicked, event invoked.");
        // exit game in build, stop play mode in editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                        Application.Quit();
#endif
    }

    private void Level1Button_Clicked()
    {
        OnStartGameButtonPressed?.Invoke();
        StartCoroutine(UITransition(/*_levelSelectPanel,*/ _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 1 button clicked");
    }

    private void Level2Button_Clicked()
    {
        OnLevelSelected?.Invoke("02BoostJumps");
        StartCoroutine(UITransition(/*_levelSelectPanel,*/ _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 2 button clicked");
    }

    private void Level3Button_Clicked()
    {
        OnLevelSelected?.Invoke("03Grapple");
        StartCoroutine(UITransition(/*_levelSelectPanel,*/ _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 3 button clicked");
    }

    private void Level4Button_Clicked()
    {
        OnLevelSelected?.Invoke("04Enemies");
        StartCoroutine(UITransition(/*_levelSelectPanel,*/ _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 4 button clicked");
    }

    private void Level5Button_Clicked()
    {
        OnLevelSelected?.Invoke("05Escape");
        StartCoroutine(UITransition(/*_levelSelectPanel,*/ _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 5 button clicked");
    }

    private void BackButton_Clicked()
    {
        StartCoroutine(UITransition(/*_levelSelectPanel,*/ _mainMenuPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Back button clicked");
    }

    private void ToggleAirJumpButton_Clicked()
    {
        OnToggleAirJumpPressed?.Invoke();
        Debug.Log("Toggle Air Jump button clicked, event invoked.");
    }

    private void AddAirJumpButton_Clicked()
    {
        OnAddAirJumpPressed?.Invoke();
        Debug.Log("Add Air Jump button clicked, event invoked.");
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

    private IEnumerator UITransition(/*VisualElement fromElement, */VisualElement toElement, float transitionLength, float holdLength)
    {
        // set the transition panel size to be the same as the screen size
        _transitionPanel.style.width = Screen.width;
        _transitionPanel.style.height = Screen.height;

        _transitionPanel.style.translate = _transitionStartPosition;

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(transitionLength) }
        };

        _transitionPanel.style.translate = _transitionMidPosition;
        yield return new WaitForSeconds(transitionLength);

        // set all children of the root visual element to display none
        foreach (VisualElement child in _uIDocument.rootVisualElement.Children())
        {
            child.style.display = DisplayStyle.None;
        }
        // set the transition panel and the toElement to display flex
        _transitionPanel.style.display = DisplayStyle.Flex;
        toElement.style.display = DisplayStyle.Flex;

        yield return new WaitForSeconds(holdLength);
        _transitionPanel.style.translate = _transitionEndPosition;
        yield return new WaitForSeconds(transitionLength);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(0) }
        };
        _transitionPanel.style.translate = _transitionStartPosition;
    }

    private IEnumerator SceneTransition(VisualElement fromElement, VisualElement toElement, float transitionLength, float holdLength)
    {
        // set the transition panel size to be the same as the screen size
        _transitionPanel.style.width = Screen.width;
        _transitionPanel.style.height = Screen.height;

        _transitionPanel.style.translate = _transitionStartPosition;

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(transitionLength) }
        };

        _transitionPanel.style.translate = _transitionMidPosition;
        yield return new WaitForSeconds(transitionLength);
        fromElement.style.display = DisplayStyle.None;
        toElement.style.display = DisplayStyle.Flex;
        OnSceneTransitionRequested?.Invoke();
        yield return new WaitForSeconds(holdLength);
        _transitionPanel.style.translate = _transitionEndPosition;
        yield return new WaitForSeconds(transitionLength);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(0) }
        };
        _transitionPanel.style.translate = _transitionStartPosition;

    }

    private IEnumerator SceneReload(float transitionLength, float holdLength)
    {
        // set the transition panel size to be the same as the screen size
        _transitionPanel.style.width = Screen.width;
        _transitionPanel.style.height = Screen.height;

        _transitionPanel.style.translate = _transitionStartPosition;

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(transitionLength) }
        };

        _transitionPanel.style.translate = _transitionMidPosition;
        yield return new WaitForSeconds(transitionLength);
        OnSceneReloadRequested?.Invoke();
        yield return new WaitForSeconds(holdLength);
        _transitionPanel.style.translate = _transitionEndPosition;
        yield return new WaitForSeconds(transitionLength);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(0) }
        };
        _transitionPanel.style.translate = _transitionStartPosition;

    }

}
