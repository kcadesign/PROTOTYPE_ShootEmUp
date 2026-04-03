using System.Collections;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float StartSize = 0f;
    public float EndSize = 1f;
    public float Duration = 1f;

    private void Start()
    {
        transform.localScale = Vector3.one * StartSize;
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            float time = Mathf.Clamp01(elapsed / Duration);
            float size = Mathf.Lerp(StartSize, EndSize, time);
            transform.localScale = Vector3.one * size;
            yield return null;
        }

        transform.localScale = Vector3.one * EndSize;
        Destroy(gameObject);
    }
}
