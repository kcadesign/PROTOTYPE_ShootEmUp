using UnityEngine;
using System.Collections;

public class Grow : MonoBehaviour
{
    public float SizeMultiplier = 0.05f;

    private void FixedUpdate()
    {
        transform.localScale += Vector3.one * SizeMultiplier;
    }
}
