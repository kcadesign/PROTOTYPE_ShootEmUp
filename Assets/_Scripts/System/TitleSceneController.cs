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

    void Update()
    {
        if (_submit != null && _submit.WasPressedThisFrame())
        {
            LoadNextScene();
        }

    }


    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
