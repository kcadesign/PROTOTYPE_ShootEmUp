using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleSceneController : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction _submit;

    private void Awake()
    {
        _submit = InputActions.FindAction("Submit");
    }

    private void OnEnable()
    {
        InputActions.FindActionMap("UI").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("UI").Disable();
    }

    //public CanvasGroup TitleSceneUIPanel;
    //private bool _keyPressed = false;

    // check if any buttons are pressed to move to the next scene
    void Update()
    {
        if(_submit != null && _submit.WasPressedThisFrame())
        {
            LoadNextScene();
        }

        //if (Input.anyKeyDown && !_keyPressed)
        //{
        //    _keyPressed = true;

        //    // cancel any ongoing tweens to avoid conflicts
        //    //LeanTween.cancel(TitleSceneUIPanel.gameObject);

        //    // load the next scene in the index
        //    //FadeOutUI();
        //    LoadNextScene();
        //}
    }

    //private void FadeInUI()
    //{
    //    TitleSceneUIPanel.alpha = 0;
    //    TitleSceneUIPanel.LeanAlpha(1, 1f).setEaseInOutSine();
    //}

    //private void FadeOutUI()
    //{
    //    TitleSceneUIPanel.LeanAlpha(0, 1f).setEaseInOutSine().setOnComplete(() =>
    //    {
    //        LoadNextScene();
    //    });
    //}

    private void LoadNextScene()
    {
        //if (_keyPressed)
        //{
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        //}
    }
}
