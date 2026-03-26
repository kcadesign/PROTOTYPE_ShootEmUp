using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject Zone2SpawnPoint; // Set this in the inspector to the spawn point of zone 2

    private void OnEnable()
    {
        UIController.OnResetButtonClicked += UIController_OnResetButtonClicked;
        UIController.OnZone2ButtonClicked += UIController_OnZone2ButtonClicked;
    }

    private void OnDisable()
    {
        UIController.OnResetButtonClicked -= UIController_OnResetButtonClicked;
        UIController.OnZone2ButtonClicked -= UIController_OnZone2ButtonClicked;
    }

    private void UIController_OnResetButtonClicked()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UIController_OnZone2ButtonClicked()
    {
        // for debugging only
        // find the player game object and set its position to the spawn point of zone 2
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = Zone2SpawnPoint.transform.position; // Change this to the actual spawn point of zone 2
        }
    }
}
