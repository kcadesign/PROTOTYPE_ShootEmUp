using Unity.Cinemachine;
using UnityEngine;

public class CameraAngleController : MonoBehaviour
{
    private CinemachinePanTilt _panTilt;

    private GameObject _target;

    private void Awake()
    {
        _panTilt = GetComponent<CinemachinePanTilt>();
   }

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
        _target = target;
    }


    private void Update()
    {
        if (_target == null)
        {
            //Debug.LogWarning("Target is null. Cannot adjust camera angle.");
            return;
        }
        else
        {
                if (_target.transform.position.x > 0)
                {
                    //_cinemachineCamera.ForceCameraPosition(_cinemachineCamera.transform.position, Quaternion.Euler(0, 15, 0));
                    Debug.Log("Target is on the right side. Setting pan tilt to 15 degrees.");
                    _panTilt.PanAxis.Value = 15f;
                    Debug.Log($"Pan tilt value set to: {_panTilt.PanAxis.Value}");
                }
                else
                {
                    //_cinemachineCamera.ForceCameraPosition(_cinemachineCamera.transform.position, Quaternion.Euler(0, -15, 0));
                    Debug.Log("Target is on the left side. Setting pan tilt to -15 degrees.");
                    _panTilt.PanAxis.Value = -15f;
                    Debug.Log($"Pan tilt value set to: {_panTilt.PanAxis.Value}");
                }
        }
    }
}
