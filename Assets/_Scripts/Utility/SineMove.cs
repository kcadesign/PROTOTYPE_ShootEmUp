using UnityEngine;

public class SineMove : MonoBehaviour
{
    private Vector3 _position;

    public float Speed = 1.0f;
    public float Frequency = 1.0f;
    public float Magnitude;


    private void Awake()
    {
        _position = transform.position;
    }

    private void Update()
    {
        MoveDown();
    }

    private void MoveDown()
    {
        _position -= transform.up * Time.deltaTime * Speed;
        transform.position = _position + transform.right * Mathf.Sin(Time.time * Frequency) * Magnitude; 
    }
}

