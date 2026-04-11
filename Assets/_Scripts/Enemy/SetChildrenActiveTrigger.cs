using UnityEngine;

public class SetChildrenActiveTrigger : MonoBehaviour
{
    private bool _isActive = false;

    private void Awake()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isActive)
        {
            _isActive = true;
            //Debug.Log("Player entered obstacle trigger zone");
            foreach (Transform child in gameObject.transform)
            {
                //Move move = child.GetComponent<Move>();
                //move.SetCanMove(true);
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
}
