using UnityEngine;

public class Rotate : MonoBehaviour
{
    public bool RotateX = false;
    public bool RotateY = false;
    public bool RotateZ = false;
    [SerializeField] private float _rotationSpeed = 1.0f;
    [SerializeField] private bool _triggerEnteredEffect = false;

    private void OnDisable()
    {
        _triggerEnteredEffect = false;
        StandardRotation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _triggerEnteredEffect = true;
        }
    }

    void Update()
    {
        if (_triggerEnteredEffect)
        {
            FastRotation();
        }
        else
        {
            StandardRotation();
        }
    }

    private void StandardRotation()
    {
        if (RotateX)
            // rotate object around X axis
            transform.Rotate(_rotationSpeed * Time.deltaTime, 0, 0);
        if (RotateY)
            // rotate object around Y axis
            transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
        if (RotateZ)
            // rotate object around Z axis
            transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);
    }

    private void FastRotation()
    {
        if (RotateX)
            // rotate object around X axis
            transform.Rotate(_rotationSpeed * -5 * Time.deltaTime, 0, 0);
        if (RotateY)
            // rotate object around Y axis
            transform.Rotate(0, _rotationSpeed * -5 * Time.deltaTime, 0);
        if (RotateZ)
            // rotate object around Z axis
            transform.Rotate(0, 0, _rotationSpeed * -5 * Time.deltaTime);
    }
}
