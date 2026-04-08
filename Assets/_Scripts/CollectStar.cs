using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CollectStar : MonoBehaviour
{
    public static event Action<int> OnCurrencyCollected;

    public int StarValue = 1;

    public float EndSize = 0.5f;
    public float VerticalMove;
    public float Duration = 1f;

    private void Start()
    {
        OnCurrencyCollected?.Invoke(StarValue);
        StartCoroutine(CollectCoroutine());
    }

    private IEnumerator CollectCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < Duration)
        {
            elapsed += Time.deltaTime;
            //float time = Mathf.Clamp01(elapsed / Duration);
            float size = Mathf.Lerp(0f, EndSize, Duration);
            transform.localScale = Vector3.one * size;

            float moveY = Mathf.Lerp(0f, VerticalMove, Duration);
            transform.localPosition += Vector3.up * moveY;
            yield return null;
        }

        Destroy(gameObject);
    }
}
