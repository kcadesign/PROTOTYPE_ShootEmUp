using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    public float DestroyDelay = 1f;

    void Start()
    {
        Destroy(gameObject, DestroyDelay);
    }

}
