using UnityEngine;

public class FloatTowards : MonoBehaviour
{
    public TriggerCollision TriggerCollision;
    private Vector3 _desiredPosition = new Vector3(1,2,0);
    public float MoveSpeed;

    private void Update()
    {
        _desiredPosition = TriggerCollision.GetCollisionPosition();
        transform.position = Vector3.MoveTowards(transform.position, _desiredPosition, MoveSpeed * Time.deltaTime);
    }


}
