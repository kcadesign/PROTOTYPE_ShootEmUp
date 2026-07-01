using System;
using System.Collections;
using UnityEngine;

public class CollectStar : MonoBehaviour
{
    public static event Action<int> OnCurrencyCollected;

    public int StarValue = 1;

    public float SizeMultiplier = 0.05f;
    public float VerticalMoveMultiplier = 0.05f;
    public float Duration = 1f;
    //public float zPosition = -10f;

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
            //float size = Mathf.Lerp(0f, EndSize, time);
            transform.localScale += Vector3.one * SizeMultiplier;

            //float moveY = Mathf.Lerp(0f, VerticalMove, time);
            transform.localPosition += Vector3.up * VerticalMoveMultiplier;
            //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, zPosition);
            yield return null;
        }

        Destroy(gameObject);
    }
}
