using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugSceneController : MonoBehaviour
{
    private GameObject Zone2SpawnPoint;

    private void OnEnable()
    {
        //UIController.OnResetButtonPressed += UIController_OnResetButtonClicked;
        UIController.OnZone2ButtonPressed += UIController_OnZone2ButtonClicked;

    }

    private void OnDisable()
    {
        //UIController.OnResetButtonPressed -= UIController_OnResetButtonClicked;
        UIController.OnZone2ButtonPressed -= UIController_OnZone2ButtonClicked;

    }

    private void UIController_OnResetButtonClicked()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void UIController_OnZone2ButtonClicked()
    {
        Zone2SpawnPoint = GameObject.Find("Zone2SpawnPoint");

        // for debugging only
        // find the player game object and set its position to the spawn point of zone 2
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = Zone2SpawnPoint.transform.position; // Change this to the actual spawn point of zone 2
        }
    }

}
