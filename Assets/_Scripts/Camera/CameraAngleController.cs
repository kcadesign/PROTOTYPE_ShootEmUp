using Unity.Cinemachine;
using UnityEngine;

public class CameraAngleController : MonoBehaviour
{
    private FindCameraTarget _findCameraTarget;
    private CinemachineCamera _cinemachineCamera;
    private CinemachinePanTilt _panTilt;

    private GameObject _target;

    private void Awake()
    {
        _cinemachineCamera = GetComponent<CinemachineCamera>();
        _panTilt = GetComponent<CinemachinePanTilt>();

        _findCameraTarget = GetComponent<FindCameraTarget>();
        if (_findCameraTarget != null)
        {
            _target = _findCameraTarget.GetCameraTarget();
        }
    }

    private void Update()
    {
        if (_target != null && _cinemachineCamera != null)
        {
            if (_target.transform.position.x > 0)
            {
                _cinemachineCamera.ForceCameraPosition(_cinemachineCamera.transform.position, Quaternion.Euler(0, 15, 0));
            }
            else  
            {
                _cinemachineCamera.ForceCameraPosition(_cinemachineCamera.transform.position, Quaternion.Euler(0, -15, 0));
            }
        }
    }
}
