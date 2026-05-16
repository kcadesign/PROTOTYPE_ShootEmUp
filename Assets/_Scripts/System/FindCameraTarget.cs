using System;
using Unity.Cinemachine;
using UnityEngine;

public class FindCameraTarget : MonoBehaviour
{
    public static event Action<GameObject> OnCameraTargetFound;

    private GameObject _targetObject;
    private CinemachineCamera _cinemachineCamera;

    private void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
    }

    private void OnEnable()
    {
        SceneController.OnLevelLoaded += SceneController_OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneController.OnLevelLoaded -= SceneController_OnLevelLoaded;
    }

    private void SceneController_OnLevelLoaded()
    {
        if (_targetObject == null)
        {
            Debug.Log("Target object not set. Attempting to find player by tag.");
            _targetObject = GameObject.FindGameObjectWithTag("Player");
            SetCameraTarget(_targetObject);
            return;
        }
        else
        {
            Debug.Log("Target object already set. Updating camera target.");
            SetCameraTarget(_targetObject);
        }
    }

    private void SetCameraTarget(GameObject targetObject)
    {
        if (_cinemachineCamera != null && targetObject != null)
        {
            _cinemachineCamera.LookAt = targetObject.transform;
            _cinemachineCamera.Follow = targetObject.transform;
            OnCameraTargetFound?.Invoke(targetObject);
        }
        else if (_cinemachineCamera == null)
        {
            Debug.LogWarning("Cinemachine Camera is null.");
        }
        else if (targetObject == null)
        {
            Debug.LogWarning("Target object is null.");
        }
    }

    public GameObject GetCameraTarget()
    {
        return _targetObject;
    }
}
