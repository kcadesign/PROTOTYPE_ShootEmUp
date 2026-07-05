using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Deactivate : MonoBehaviour
{
    private Collider2D _collider;
    public GameObject GrappleActiveRender;
    public GameObject GrappleInactiveRender;

    private bool _isUsed = false;
    private float _timer = 0f;
    public float RespawnDelay = 3f;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        if (_collider == null)
        {
            Debug.LogWarning("No Collider2D component found on " + name);
        }
    }

    private void Update()
    {
        if (!_isUsed)
        {
            _timer = 0f;
        }
        else if (_isUsed)
        {
            _timer += Time.deltaTime;

            if (_timer >= RespawnDelay)
            {
                ResetObject();
                _timer = 0f;
            }
        }


    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponentInChildren<Grapple>().GetIsGrappling())
            {
                ObjectUsed();
            }
        }
    }

    public void ObjectUsed()
    {
        _isUsed = true;
        _collider.enabled = false;

        // turn on outline render
        GrappleActiveRender.SetActive(false);
        GrappleInactiveRender.SetActive(true);
    }

    public void ResetObject()
    {
        _isUsed = false;
        _collider.enabled = true;

        // turn on base render and turn off outline render
        GrappleInactiveRender.SetActive(false);
        GrappleActiveRender.SetActive(true);
    }

}
