using UnityEngine;

public class SineMove : MonoBehaviour
{
    private Vector3 _position;

    public float VerticalSpeed = 1.0f;
    public float Frequency = 1.0f;
    public float Magnitude;


    private void Awake()
    {
        _position = transform.position;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _position += transform.up * Time.deltaTime * VerticalSpeed;
        transform.position = _position + transform.right * Mathf.Sin(Time.time * Frequency) * Magnitude; 
    }
}

