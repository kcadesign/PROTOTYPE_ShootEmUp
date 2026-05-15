using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject _targetObject;

    private void OnEnable()
    {
        SceneController.OnLevelLoaded += SceneController_OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneController.OnLevelLoaded -= SceneController_OnLevelLoaded;
    }

    private void Update()
    {
        FollowTarget();
    }

    private void SceneController_OnLevelLoaded()
    {
        if (_targetObject == null)
        {
            Debug.Log("Target object not set. Attempting to find player by tag.");
            _targetObject = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        else
        {
            Debug.Log("Target object already set. Updating camera target.");
        }
    }

    private void FollowTarget()
    {
        if (_targetObject == null)
        {
            Debug.LogWarning("Target object is null. Cannot follow target.");
            return;
        }
        gameObject.transform.position = _targetObject.transform.position;
    }

}
