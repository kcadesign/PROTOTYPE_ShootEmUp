using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public static event Action OnGameStarted;
    public static event Action OnLevelLoaded;

    private AsyncOperation _LoadFirstLevelAsyncOperation;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();

    private float _loadingCountdown = 3f;
    private bool _initialScenesLoaded = false;

    private int _requestedSceneToLoad = 0;

    private void Awake()
    {
        _scenesToLoad.Clear();
        _requestedSceneToLoad = 0;
    }

    private void OnEnable()
    {
        //TutorialInput.OnSubmitPressedEvent += TutorialInput_OnSubmitPressedEvent;
        //HandleGameState.OnSceneReloadRequested += HandleGameState_OnSceneReloadRequested;

        UIController.OnStartGameButtonPressed += UIController_OnStartGameButtonPressed;
        UIController.OnSceneTransitionRequested += UIController_OnSceneTrasitionRequested;
        UIController.OnSceneReloadRequested += UIController_OnSceneReloadRequested;

        LevelEnd.OnPlayerEnterLevelEnd += LevelEnd_OnPlayerEnterLevelEnd;

    }

    private void OnDisable()
    {
        //TutorialInput.OnSubmitPressedEvent -= TutorialInput_OnSubmitPressedEvent;
        //HandleGameState.OnSceneReloadRequested -= HandleGameState_OnSceneReloadRequested;

        UIController.OnStartGameButtonPressed -= UIController_OnStartGameButtonPressed;
        UIController.OnSceneTransitionRequested -= UIController_OnSceneTrasitionRequested;
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


        StartCoroutine(LoadingCountdownCoroutine());
    }

    private IEnumerator LoadingCountdownCoroutine()
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
        _LoadFirstLevelAsyncOperation = SceneManager.LoadSceneAsync("01BasicMovement", LoadSceneMode.Additive);
        _LoadFirstLevelAsyncOperation.allowSceneActivation = false;

        if (_LoadFirstLevelAsyncOperation != null)
        {
            _LoadFirstLevelAsyncOperation.allowSceneActivation = true;
            while (!_LoadFirstLevelAsyncOperation.isDone)
            {
                await Task.Yield();
            }

            // Set firstLevelScene as the active scene
            Scene firstLevelScene = SceneManager.GetSceneByName("01BasicMovement");
            if (firstLevelScene.IsValid())
                SceneManager.SetActiveScene(firstLevelScene);

            Debug.Log("First level activated.");
        }

        OnGameStarted?.Invoke();
        OnLevelLoaded?.Invoke();
    }

    private async void UIController_OnSceneTrasitionRequested()
    {
        AsyncOperation loadNewLevelOperation = SceneManager.LoadSceneAsync(_requestedSceneToLoad, LoadSceneMode.Additive);
        loadNewLevelOperation.allowSceneActivation = false;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        if (loadNewLevelOperation != null)
        {
            loadNewLevelOperation.allowSceneActivation = true;
            while (!loadNewLevelOperation.isDone)
            {
                await Task.Yield();
            }

            // Set newScene as the active scene
            Scene newScene = SceneManager.GetSceneByBuildIndex(_requestedSceneToLoad);
            if (newScene.IsValid())
                SceneManager.SetActiveScene(newScene);

            Debug.Log("New level activated.");
        }
        OnLevelLoaded?.Invoke();
    }

    private async void UIController_OnSceneReloadRequested()
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
            Scene s = SceneManager.GetSceneAt(i);
            if (s.name == currentLevel.name && s.handle != originalHandle)
            {
                reloadedScene = s;
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

    private void LevelEnd_OnPlayerEnterLevelEnd(int nextSceneIndex)
    {
        _requestedSceneToLoad = nextSceneIndex;
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
