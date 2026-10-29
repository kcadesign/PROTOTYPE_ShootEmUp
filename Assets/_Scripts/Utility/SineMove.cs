using UnityEngine;

public class SineMove : MonoBehaviour
{
    private Vector3 _position;

    public float VerticalFrequency = 1.0f;
    public float VerticalMagnitude = 1.0f;
    public float HorizontalFrequency = 1.0f;
    public float HorizontalMagnitude = 1.0f;


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
        transform.position = _position + new Vector3(Mathf.Sin(Time.time * HorizontalFrequency) * HorizontalMagnitude, 
            Mathf.Sin(Time.time * VerticalFrequency) * VerticalMagnitude, 0);
    }
}

