using Unity.Cinemachine;
using UnityEngine;

public class CameraAngleController : MonoBehaviour
{
    private CinemachinePanTilt _panTilt;

    private GameObject _target;
    public float AngleChangeSpeed = 5f;
    public float MaxPanAngle = 15f;
    // The distance from center at which the pan reaches MaxPanAngle
    public float MaxPanDistance = 10f;

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
            // Map the target's X position to an angle between -MaxPanAngle and +MaxPanAngle.
            // When the target is at or beyond MaxPanDistance the pan reaches the full MaxPanAngle.
            float targetX = _target.transform.position.x;
            float normalized = Mathf.Clamp01(Mathf.Abs(targetX) / MaxPanDistance);
            float desiredAngle = Mathf.Sign(targetX) * MaxPanAngle * normalized;

            // Smoothly move current pan toward the desired angle
            _panTilt.PanAxis.Value = Mathf.Lerp(_panTilt.PanAxis.Value, desiredAngle, Time.deltaTime * AngleChangeSpeed);
        }
    }
}
