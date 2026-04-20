using UnityEngine;

public class TallyStars : MonoBehaviour
{
    private int _totalStarCount = 0;

    private void OnEnable()
    {
        CollectStar.OnCurrencyCollected += CollectStar_OnCurrencyCollected;
    }

    private void OnDisable()
    {
        CollectStar.OnCurrencyCollected -= CollectStar_OnCurrencyCollected;
    }

    private void CollectStar_OnCurrencyCollected(int starValue)
    {
        _totalStarCount += starValue;
        Debug.Log("Total Stars Collected: " + _totalStarCount);
    }
}
