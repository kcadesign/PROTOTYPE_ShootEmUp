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

    public static event Action OnRetryButtonPressed;
    public static event Action OnMainMenuButtonPressed;

    public static event Action<int> OnCardSelected; // Pass the index of the selected card (0, 1, or 2)

    public static event Action OnNextLevelRequested;
    public static event Action OnSceneReloadRequested;

    public static event Action<bool> OnPauseMenuActive;

    [Header("References")]
    public InputActionAsset InputActions;
    public PlayerStats PlayerStatsData;
    public CardUI CardUIData;
    [SerializeField] private UIDocument _uIDocument;

    // Panels
    private VisualElement _mainMenuPanel;
    private VisualElement _levelSelectPanel;
    private VisualElement _transitionPanel;
    private VisualElement _HUDPanel;
    private VisualElement _boonChoicePanel;
    private VisualElement _XPPanel;
    private VisualElement _pausePanel;

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

    // Boon choice menu
    private Button _choice1Button;
    private Button _choice2Button;
    private Button _choice3Button;
    private Button _skipChoiceButton;

    // Run end menu
    private Button _retryButton;
    private Button _mainMenu;

    // Pause menu
    private Button _resumeButton;
    private InputAction _cancel;
    private bool _isPauseMenuActive = false;
    private bool _canPause = false;



    [Header("Scene Transition")]
    public float TransitionLength = 1f;
    public float TransitionHoldLength = 2f;
    public Translate _transitionStartPosition = new Translate(0, Screen.height, 0);
    public Translate _transitionMidPosition = new Translate(0, 0, 0);
    public Translate _transitionEndPosition = new Translate(0, -Screen.height, 0);

    private void Awake()
    {
        _cancel = InputActions.FindAction("Cancel");

        _mainMenuPanel = _uIDocument.rootVisualElement.Q<VisualElement>("MainMenu");
        _levelSelectPanel = _uIDocument.rootVisualElement.Q<VisualElement>("LevelSelectMenu");
        _transitionPanel = _uIDocument.rootVisualElement.Q<VisualElement>("Transition");
        _HUDPanel = _uIDocument.rootVisualElement.Q<VisualElement>("HUD");
        _XPPanel = _uIDocument.rootVisualElement.Q<VisualElement>("Experience");
        _boonChoicePanel = _uIDocument.rootVisualElement.Q<VisualElement>("BoonChoice");
        _pausePanel = _uIDocument.rootVisualElement.Q<VisualElement>("PauseMenu");

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

        _choice1Button = _uIDocument.rootVisualElement.Q<Button>("Choice1Button");
        _choice2Button = _uIDocument.rootVisualElement.Q<Button>("Choice2Button");
        _choice3Button = _uIDocument.rootVisualElement.Q<Button>("Choice3Button");
        _skipChoiceButton = _uIDocument.rootVisualElement.Q<Button>("SkipButton");

        _retryButton = _uIDocument.rootVisualElement.Q<Button>("RetryButton");
        _mainMenu = _uIDocument.rootVisualElement.Q<Button>("MainMenuButton");

        _resumeButton = _uIDocument.rootVisualElement.Q<Button>("ResumeButton");

    }

    private void OnEnable()
    {
        InputActions.FindActionMap("UI").Enable();

        HandleGameState.OnGameStateChanged += HandleGameState_OnGameStateChanged;

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

        _choice1Button.clicked += Choice1Button_Clicked;
        _choice2Button.clicked += Choice2Button_Clicked;
        _choice3Button.clicked += Choice3Button_Clicked;
        _skipChoiceButton.clicked += SkipChoiceButton_Clicked;

        _retryButton.clicked += RetryButton_clicked;
        _mainMenu.clicked += MainMenuButton_Clicked;

        _resumeButton.clicked += ResumeButton_Clicked;

        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("UI").Disable();

        HandleGameState.OnGameStateChanged -= HandleGameState_OnGameStateChanged;

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

        _choice1Button.clicked -= Choice1Button_Clicked;
        _choice2Button.clicked -= Choice2Button_Clicked;
        _choice3Button.clicked -= Choice3Button_Clicked;
        _skipChoiceButton.clicked -= SkipChoiceButton_Clicked;

        _retryButton.clicked -= RetryButton_clicked;
        _mainMenu.clicked -= MainMenuButton_Clicked;

        _resumeButton.clicked -= ResumeButton_Clicked;

        LevelEnd.OnPlayerEnterLevelEnd -= LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void Start()
    {
        StartCoroutine(Transition(_mainMenuPanel, TransitionLength, TransitionHoldLength));
        GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("StartButton").Focus();
    }

    private void Update()
    {
        // FIX: Replace with consistent pause input across controlers (KB = Escape, Gamepad = Options)
        if (_cancel != null && _cancel.WasPressedThisFrame() && !_isPauseMenuActive && _canPause)
        {
            OnPauseMenuActive?.Invoke(true);
            ShowUIPanel(_pausePanel);
            _isPauseMenuActive = true;
        }
        else if (_cancel != null && _cancel.WasPressedThisFrame() && _isPauseMenuActive && _canPause)
        {
            OnPauseMenuActive?.Invoke(false);
            ShowUIPanel(_HUDPanel);
            _isPauseMenuActive = false;
        }
    }

    private void HandleGameState_OnGameStateChanged(HandleGameState.GameState newState)
    {
        switch (newState)
        {
            case HandleGameState.GameState.PreGameMenu:
                _canPause = false;
                break;
            case HandleGameState.GameState.Transition:
                _canPause = false;
                break;
            case HandleGameState.GameState.LevelStart:
                _canPause = false;
                break;
            case HandleGameState.GameState.Gameplay:
                _canPause = true;
                break;
            case HandleGameState.GameState.GamePaused:
                _canPause = true;
                break;
            case HandleGameState.GameState.Shop:
                _canPause = false;
                break;
            case HandleGameState.GameState.LevelEnd:
                _canPause = false;
                break;
            case HandleGameState.GameState.ChoosePowerup:
                _canPause = false;
                break;
            case HandleGameState.GameState.BossFight:
                _canPause = true;
                break;
            case HandleGameState.GameState.RunEnd:
                _canPause = false;
                StartCoroutine(Transition(_XPPanel, TransitionLength, TransitionHoldLength));
                break;
            case HandleGameState.GameState.XPTally:
                _canPause = false;
                break;
            case HandleGameState.GameState.GameRestart:
                StartCoroutine(Transition(_mainMenuPanel, TransitionLength, TransitionHoldLength));
                _canPause = false;
                break;
            case HandleGameState.GameState.GameFinished:
                _canPause = false;
                break;
            case HandleGameState.GameState.Credits:
                _canPause = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
    }

    private void StartGameButton_Clicked()
    {
        OnStartGameButtonPressed?.Invoke();
        StartCoroutine(Transition(_HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Start Game button clicked");
    }

    private void LevelSelectButton_Clicked()
    {
        StartCoroutine(Transition(_levelSelectPanel, TransitionLength, TransitionHoldLength));
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

    // level select for debug only
    private void Level1Button_Clicked()
    {
        OnStartGameButtonPressed?.Invoke();
        StartCoroutine(Transition(_HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 1 button clicked");
    }

    private void Level2Button_Clicked()
    {
        OnLevelSelected?.Invoke("02BoostJumps");
        StartCoroutine(Transition(_HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 2 button clicked");
    }

    private void Level3Button_Clicked()
    {
        OnLevelSelected?.Invoke("03Grapple");
        StartCoroutine(Transition(_HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 3 button clicked");
    }

    private void Level4Button_Clicked()
    {
        OnLevelSelected?.Invoke("04Enemies");
        StartCoroutine(Transition(_HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 4 button clicked");
    }

    private void Level5Button_Clicked()
    {
        OnLevelSelected?.Invoke("05Escape");
        StartCoroutine(Transition(_HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Level 5 button clicked");
    }

    private void BackButton_Clicked()
    {
        StartCoroutine(Transition(_mainMenuPanel, TransitionLength, TransitionHoldLength));
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

    private void Choice1Button_Clicked()
    {
        int card1Cost = CardUIData.GetCardCost(0);
        if (card1Cost > PlayerStatsData.GetCurrentCurrency())
        {
            Debug.Log("Not enough currency to purchase Card 1");
            return;
        }
        Debug.Log("Choice 1 button clicked");
        OnCardSelected?.Invoke(0); // Invoke the event with the index of the selected card
        PlayerStatsData.SubtractFromCurrentCurrency(card1Cost); // subtract used currency from player stats
        StartCoroutine(Transition(OnNextLevelRequested, _HUDPanel, TransitionLength, TransitionHoldLength));
    }

    private void Choice2Button_Clicked()
    {
        int card2Cost = CardUIData.GetCardCost(1);
        if (card2Cost > PlayerStatsData.GetCurrentCurrency())
        {
            Debug.Log("Not enough currency to purchase Card 2");
            return;
        }
        Debug.Log("Choice 2 button clicked");
        OnCardSelected?.Invoke(1); // Invoke the event with the index of the selected card
        PlayerStatsData.SubtractFromCurrentCurrency(card2Cost); // subtract used currency from player stats
        StartCoroutine(Transition(OnNextLevelRequested, _HUDPanel, TransitionLength, TransitionHoldLength));
    }

    private void Choice3Button_Clicked()
    {
        int card3Cost = CardUIData.GetCardCost(2);
        if (card3Cost > PlayerStatsData.GetCurrentCurrency())
        {
            Debug.Log("Not enough currency to purchase Card 3");
            return;
        }
        Debug.Log("Choice 3 button clicked");
        OnCardSelected?.Invoke(2); // Invoke the event with the index of the selected card
        PlayerStatsData.SubtractFromCurrentCurrency(card3Cost); // subtract used currency from player stats
        StartCoroutine(Transition(OnNextLevelRequested, _HUDPanel, TransitionLength, TransitionHoldLength));
    }

    private void SkipChoiceButton_Clicked()
    {
        Debug.Log("Skip Choice button clicked");
        StartCoroutine(Transition(OnNextLevelRequested, _HUDPanel, TransitionLength, TransitionHoldLength));
    }

    private void RetryButton_clicked()
    {
        StartCoroutine(Transition(OnRetryButtonPressed, _HUDPanel, TransitionLength, TransitionHoldLength));
        Debug.Log("Retry button clicked, event invoked.");
    }

    private void MainMenuButton_Clicked()
    {
        Debug.Log("Main Menu button clicked, event invoked.");
        StartCoroutine(Transition(OnMainMenuButtonPressed, _mainMenuPanel, TransitionLength, TransitionHoldLength));
    }

    private void ResumeButton_Clicked()
    {
        // hide pause menu, show HUD, invoke event to unpause game
        ShowUIPanel(_HUDPanel);
        OnPauseMenuActive?.Invoke(false);
        _isPauseMenuActive = false;
    }

    private void LevelEnd_OnPlayerEnterLevelEnd()
    {
        StartCoroutine(Transition(_boonChoicePanel, TransitionLength, TransitionHoldLength));
    }

    private IEnumerator Transition(VisualElement toElement, float transitionLength, float holdLength)
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

    private IEnumerator Transition(Action action, VisualElement toElement, float transitionLength, float holdLength)
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

        action?.Invoke();

        yield return new WaitForSeconds(holdLength);
        _transitionPanel.style.translate = _transitionEndPosition;
        yield return new WaitForSeconds(transitionLength);

        _transitionPanel.style.transitionDuration = new StyleList<TimeValue>
        {
            value = new List<TimeValue> { TimeValue.Seconds(0) }
        };
        _transitionPanel.style.translate = _transitionStartPosition;
    }

    private IEnumerator SceneReload(float transitionLength, float holdLength) // for debug only
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

    private void ShowUIPanel(VisualElement panelToShow)
    {
        // set all children of the root visual element to display none
        foreach (VisualElement child in _uIDocument.rootVisualElement.Children())
        {
            child.style.display = DisplayStyle.None;
        }
        // set the panelToShow to display flex
        panelToShow.style.display = DisplayStyle.Flex;
    }
}
