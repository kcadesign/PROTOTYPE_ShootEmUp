using UnityEngine;

public class Deactivate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponentInChildren<Grapple>().GetIsGrappling())
            {
                gameObject.SetActive(false);
            }
        }
    }

    // NEEDS TO BE IMPLEMENTED
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (collision.GetComponentInChildren<Grapple>().GetIsGrappling())
    //        {
    //            //gameObject.SetActive(false);
    //            // turn off collider and base render
    //            ObjectUsed();
    //        }
    //    }
    //}

    //public void ObjectUsed()
    //{
    //    _isUsed = true;
    //    gameObject.GetComponent<Collider2D>().enabled = false;
    //    SpriteRenderer baseRender = gameObject.GetComponentInChildren<SpriteRenderer>();
    //    // turn on outline render
    //}

    //public void ResetObject()
    //{
    //    _isUsed = false;
    //    gameObject.GetComponent<Collider2D>().enabled = true;
    //    SpriteRenderer baseRender = gameObject.GetComponentInChildren<SpriteRenderer>();
    //    // turn on base render and turn off outline render
    //}

}
