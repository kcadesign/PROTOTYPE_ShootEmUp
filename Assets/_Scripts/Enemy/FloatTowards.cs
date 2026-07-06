using UnityEngine;

public class FloatTowards : MonoBehaviour
{
    public TriggerCollision TriggerCollision;
    private Vector3 _desiredPosition;
    public float MoveSpeed;

    private void Awake()
    {
        _desiredPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (TriggerCollision != null)
        {
            _desiredPosition = TriggerCollision.GetTargetPosition();
            transform.position = Vector3.MoveTowards(transform.position, _desiredPosition, MoveSpeed * Time.deltaTime);
        }
    }


}
