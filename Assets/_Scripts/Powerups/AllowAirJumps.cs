using UnityEngine;

public class AllowAirJumps : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Jump>().SetAllowAirJumps(true);
            collision.GetComponent<Jump>().IncreaseMaxAirJumps();
            Collected(true);
            gameObject.SetActive(false);
        }
    }

    public bool Collected(bool collected)
    {
        return collected;
    }
}
