using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class SceneController : MonoBehaviour
{
    //public static event Action OnGameStarted;
    public static event Action OnLevelLoaded;
    public static event Action OnNoMoreLevels;

    private AsyncOperation _LoadFirstLevelAsyncOperation;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    private float _loadingCountdown = 2f;
    private bool _initialScenesLoaded = false;

    private int _nextSceneToLoad = 0;
    private bool _isSceneToLoad = false;

    private void Awake()
    {
        _scenesToLoad.Clear();
        _nextSceneToLoad = 0;
    }

    private void OnEnable()
    {
        //TutorialInput.OnSubmitPressedEvent += TutorialInput_OnSubmitPressedEvent;
        //HandleGameState.OnSceneReloadRequested += HandleGameState_OnSceneReloadRequested;

        UIController.OnStartGameButtonPressed += UIController_OnStartGameButtonPressed;
        UIController.OnLevelSelected += UIController_OnLevelSelected;
        UIController.OnRetryButtonPressed += UIController_OnRetryButtonPressed;
        UIController.OnMainMenuButtonPressed += UIController_OnMainMenuButtonPressed;
        UIController.OnNextLevelRequested += UIController_OnNextLevelRequested;
        UIController.OnSceneReloadRequested += UIController_OnSceneReloadRequested;

        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void OnDisable()
    {
        //TutorialInput.OnSubmitPressedEvent -= TutorialInput_OnSubmitPressedEvent;
        //HandleGameState.OnSceneReloadRequested -= HandleGameState_OnSceneReloadRequested;

        UIController.OnStartGameButtonPressed -= UIController_OnStartGameButtonPressed;
        UIController.OnLevelSelected -= UIController_OnLevelSelected;
        UIController.OnRetryButtonPressed -= UIController_OnRetryButtonPressed;
        UIController.OnMainMenuButtonPressed -= UIController_OnMainMenuButtonPressed;
        UIController.OnNextLevelRequested -= UIController_OnNextLevelRequested;
        UIController.OnSceneReloadRequested -= UIController_OnSceneReloadRequested;

        LevelEnd.OnPlayerEnterLevelEnd -= LevelEnd_OnPlayerEnterLevelEnd;
    }

    private void Start()
    {
        SceneManager.LoadSceneAsync("Loading", LoadSceneMode.Additive);

        _scenesToLoad.Add(SceneManager.LoadSceneAsync("Systems", LoadSceneMode.Additive));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync("Cameras", LoadSceneMode.Additive));
        foreach (AsyncOperation operation in _scenesToLoad)
        {
            operation.allowSceneActivation = false;
        }

        StartCoroutine(LoadingCountdown());
    }

    private IEnumerator LoadingCountdown()
    {
        while (_loadingCountdown > 0f && !_initialScenesLoaded)
        {
            _loadingCountdown -= Time.deltaTime;
            yield return null; // let Unity continue frames
        }

        if (!_initialScenesLoaded)
        {
            Debug.Log("Loading countdown finished");
            _initialScenesLoaded = true;

            SceneManager.UnloadSceneAsync("Loading");
            foreach (AsyncOperation operation in _scenesToLoad)
            {
                if (operation != null && !operation.isDone)
                    operation.allowSceneActivation = true;
            }
        }
    }

    private async void UIController_OnStartGameButtonPressed()
    {
        try
        {
            await LoadLevelAdditive("LevelGenerationTest1-1");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async Task LoadLevelAdditive(string sceneName)
    {
        try
        {
            // Load by name (use the sceneName parameter)
            _LoadFirstLevelAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            if (_LoadFirstLevelAsyncOperation == null)
            {
                Debug.LogWarning($"Failed to start loading scene '{sceneName}'.");
                return;
            }

            _LoadFirstLevelAsyncOperation.allowSceneActivation = true;
            while (!_LoadFirstLevelAsyncOperation.isDone)
                await Task.Yield();

            Scene firstLevelScene = SceneManager.GetSceneByName(sceneName);
            if (firstLevelScene.IsValid())
                SceneManager.SetActiveScene(firstLevelScene);

            Debug.Log("First level activated.");
            OnLevelLoaded?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async void UIController_OnLevelSelected(string sceneName) // DEBUG only
    {
        await LoadLevelAdditive(sceneName);
    }

    private async void UIController_OnRetryButtonPressed()
    {
        UnloadActiveScene();

        try
        {
            await LoadLevelAdditive("LevelGenerationTest1-1");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }

    }

    private async void UIController_OnMainMenuButtonPressed()
    {
        UnloadActiveScene();
    }

    private static void UnloadActiveScene()
    {
        Debug.Log($"Unloading active scene: {SceneManager.GetActiveScene().name}");
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    private async Task LoadLevelAdditiveReplace(int sceneIndex)
    {
        try
        {
            // Capture the scene we will unload AFTER the new scene is active
            Scene sceneToUnload = SceneManager.GetActiveScene();

            // Start loading the new scene additively
            AsyncOperation loadNewLevelOperation = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
            if (loadNewLevelOperation == null)
            {
                Debug.LogWarning($"Failed to start loading scene index {sceneIndex}.");
                return;
            }

            loadNewLevelOperation.allowSceneActivation = true;
            while (!loadNewLevelOperation.isDone)
                await Task.Yield();

            // Find the new scene instance and set it active
            Scene newScene = SceneManager.GetSceneByBuildIndex(sceneIndex);
            if (newScene.IsValid())
                SceneManager.SetActiveScene(newScene);

            // Now safely unload the previous active scene
            if (sceneToUnload.IsValid())
            {
                AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneToUnload);
                if (unloadOp != null)
                {
                    while (!unloadOp.isDone)
                        await Task.Yield();
                }
            }

            Debug.Log($"New level ({sceneIndex}) activated.");
            OnLevelLoaded?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async void UIController_OnNextLevelRequested()
    {
        Debug.Log($"Next level requested: {_nextSceneToLoad}");
        if (!_isSceneToLoad)
        {
            OnNoMoreLevels?.Invoke();
            //unload the current active scene and return to the main menu
            UnloadActiveScene();
        }
        else
        {
            await LoadLevelAdditiveReplace(_nextSceneToLoad);
        }
    }

    private async void UIController_OnSceneReloadRequested() // DEBUG only
    {
        Scene currentLevel = SceneManager.GetActiveScene();
        int sceneToReloadIndex = currentLevel.buildIndex;
        int originalHandle = currentLevel.handle;

        AsyncOperation reloadCurrentLevel = SceneManager.LoadSceneAsync(sceneToReloadIndex, LoadSceneMode.Additive);
        if (reloadCurrentLevel == null)
        {
            Debug.LogWarning("Failed to start reload operation.");
            return;
        }

        // Let the scene load (allowSceneActivation true so it completes)
        reloadCurrentLevel.allowSceneActivation = true;
        while (!reloadCurrentLevel.isDone)
        {
            await Task.Yield();
        }

        // Find the newly loaded scene instance (same name but different handle)
        Scene reloadedScene = default;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == currentLevel.name && scene.handle != originalHandle)
            {
                reloadedScene = scene;
                break;
            }
        }

        if (reloadedScene.IsValid())
        {
            SceneManager.SetActiveScene(reloadedScene);

            // Unload the original scene instance
            AsyncOperation unloadOriginalScene = SceneManager.UnloadSceneAsync(currentLevel);
            if (unloadOriginalScene != null)
            {
                while (!unloadOriginalScene.isDone)
                {
                    await Task.Yield();
                }
            }

            Debug.Log("Level reloaded and activated.");
        }
        else
        {
            Debug.LogWarning("Reloaded scene not found; original scene remains active.");
        }

        OnLevelLoaded?.Invoke();
    }

    private void LevelEnd_OnPlayerEnterLevelEnd()
    {
        int nextSceneIndex;
        // if the current scene is the last level, unload the current scene and return to the main menu
        if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1)
        {
            _isSceneToLoad = false;
            return;
        }
        else
        {
            _isSceneToLoad = true;
            nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            _nextSceneToLoad = nextSceneIndex;
        }
    }


    //private async void TutorialInput_OnSubmitPressedEvent()
    //{
    //    // Activate the next scene
    //    if (_standardModeOperation != null)
    //    {
    //        _standardModeOperation.allowSceneActivation = true;
    //        while (!_standardModeOperation.isDone)
    //        {
    //            await Task.Yield();
    //        }

    //        // Set StandardMode as the active scene
    //        Scene standardModeScene = SceneManager.GetSceneByName("StandardMode");
    //        if (standardModeScene.IsValid())
    //            SceneManager.SetActiveScene(standardModeScene);

    //        Debug.Log("StandardMode activated.");
    //    }

    //    // Unload the tutorial scene AFTER activation
    //    await UnloadSceneAsync("Tutorial");

    //    OnGameStarted?.Invoke();
    //}

    //private async void HandleGameState_OnSceneReloadRequested()
    //{
    //    await UnloadSceneAsync("StandardMode");
    //    //await UnloadSceneAsync("Systems");
    //    await LoadSceneAsync("StandardMode");
    //    //await LoadSceneAsync("Systems");
    //    // set the active scene to StandardMode
    //    Scene standardModeScene = SceneManager.GetSceneByName("StandardMode");
    //    if (standardModeScene.IsValid())
    //    {
    //        SceneManager.SetActiveScene(standardModeScene);
    //    }
    //}

    //public async Task LoadSceneAsync(string sceneName)
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    //    asyncLoad.allowSceneActivation = true;
    //    while (!asyncLoad.isDone)
    //    {
    //        //Debug.Log($"Loading {sceneName}: {asyncLoad.progress * 100}% complete");
    //        await Task.Yield();
    //    }
    //    Debug.Log($"{sceneName} loaded successfully.");
    //}

    //public async Task UnloadSceneAsync(string sceneName)
    //{
    //    AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
    //    while (!asyncUnload.isDone)
    //    {
    //        await Task.Yield();
    //    }

    //    Debug.Log($"{sceneName} unloaded successfully.");
    //}

    //public async Task PreloadScene(string sceneName)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    //    operation.allowSceneActivation = false;
    //    while (operation.progress < 0.9f)
    //    {
    //        await Task.Yield();
    //    }

    //    Debug.Log($"{sceneName} preloaded.");
    //}

    //public async Task ActivateScene(AsyncOperation operation)
    //{
    //    Debug.Log(operation + " is being activated.");
    //    if (operation != null)
    //    {
    //        operation.allowSceneActivation = true;
    //        while (!operation.isDone)
    //        {
    //            await Task.Yield();
    //        }

    //        Debug.Log("Scene activated.");
    //    }
    //}

}
