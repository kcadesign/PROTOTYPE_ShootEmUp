using UnityEngine;

public class ConstrainPosition : MonoBehaviour
{
    [Header("Position Constraints")]
    public bool ConstrainPositionX = false;
    public float ConstrainPositionXValue = 0f;
    public bool ConstrainPositionY = false;
    public float ConstrainPositionYValue = 0f;
    public bool ConstrainPositionZ = false;
    public float ConstrainPositionZValue = 0f;

    [Header("Lowest Position")]
    public float LowestYPosition = -10f;

    // Update is called once per frame
    void Update()
    {
        if (ConstrainPositionX || ConstrainPositionY || ConstrainPositionZ)
        {
            Vector3 position = transform.position;
            if (ConstrainPositionX)
            {
                position.x = ConstrainPositionXValue;
            }
            if (ConstrainPositionY)
            {
                position.y = ConstrainPositionYValue;
            }
            if (ConstrainPositionZ)
            {
                position.z = ConstrainPositionZValue;
            }
            transform.position = position;
        }

        if (transform.position.y < LowestYPosition)
        {
            Vector3 position = transform.position;
            position.y = LowestYPosition;
            transform.position = position;
        }
    }
}
