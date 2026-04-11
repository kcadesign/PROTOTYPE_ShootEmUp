using UnityEngine;

public class FloatTowards : MonoBehaviour
{
    public TriggerCollision TriggerCollision;
    private Vector3 _desiredPosition;
    public float MoveSpeed;

    private void Awake()
    {
        _desiredPosition = new Vector3(transform.position.x, 0, 0);
    }

    private void Update()
    {
        _desiredPosition = TriggerCollision.GetTargetPosition();
        transform.position = Vector3.MoveTowards(transform.position, _desiredPosition, MoveSpeed * Time.deltaTime);
    }


}
