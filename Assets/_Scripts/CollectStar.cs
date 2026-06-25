using System;
using System.Collections;
using UnityEngine;

public class CollectStar : MonoBehaviour
{
    public static event Action<int> OnCurrencyCollected;

    public int StarValue = 1;

    public float EndSize = 0.5f;
    public float VerticalMove;
    public float Duration = 1f;
    public float zPosition = -10f;

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
            float time = Mathf.Clamp01(elapsed / Duration);
            float size = Mathf.Lerp(0f, EndSize, time);
            transform.localScale = Vector3.one * size;

            float moveY = Mathf.Lerp(0f, VerticalMove, time);
            transform.localPosition += Vector3.up * moveY;
            //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zPosition);
            yield return null;
        }

        Destroy(gameObject);
    }
}
