using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private void OnEnable()
    {
        UIController.OnResetButtonClicked += UIController_OnResetButtonClicked;
    }

    private void OnDisable()
    {
        UIController.OnResetButtonClicked -= UIController_OnResetButtonClicked;
    }

    private void UIController_OnResetButtonClicked()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
