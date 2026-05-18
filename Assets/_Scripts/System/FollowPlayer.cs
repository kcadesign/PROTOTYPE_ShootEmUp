using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject _targetObject;

    private void OnEnable()
    {
        FindCameraTarget.OnCameraTargetFound += FindCameraTarget_OnCameraTargetFound;
    }

    private void OnDisable()
    {
        FindCameraTarget.OnCameraTargetFound -= FindCameraTarget_OnCameraTargetFound;
    }

    private void FindCameraTarget_OnCameraTargetFound(GameObject target)
    {
        _targetObject = target;
    }

    private void Update()
    {
        if (_targetObject == null) return;
                FollowTarget();
    }

    //private void SceneController_OnLevelLoaded()
    //{
    //    if (_targetObject == null)
    //    {
    //        Debug.Log("Target object not set. Attempting to find player by tag.");
    //        _targetObject = GameObject.FindGameObjectWithTag("Player");
    //        if (_targetObject == null)
    //        {
    //            Debug.LogWarning("Player object not found with tag 'Player'. Cannot set target.");
    //        }
    //        else if (_targetObject != null)
    //        {
    //            Debug.Log("Player object found. Setting target to player.");
    //        }
    //            return;
    //    }
    //    else
    //    {
    //        Debug.Log("Target object already set. Updating camera target.");
    //    }
    //}

    private void FollowTarget()
    {
        if (_targetObject == null)
        {
            Debug.LogWarning("Target object is null. Cannot follow target.");
            return;
        }
        gameObject.transform.position = new Vector3(_targetObject.transform.position.x, _targetObject.transform.position.y, gameObject.transform.position.z);
    }

}
