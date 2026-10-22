using UnityEngine;

public class Collapse : MonoBehaviour
{
    private Collider2D _collider;
    public GameObject ActiveRender;
    public GameObject InactiveRender;

    private bool _collapseTriggered = false;
    private float _collapseTimer = 0f;
    public float CollapseDelay = 3f;
    private float _respawnTimer = 0f;
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
        if (!_collapseTriggered)
        {
            _collapseTimer = 0f;
        }
        else if (_collapseTriggered)
        {
            _collapseTimer += Time.deltaTime;

            if (_collapseTimer >= CollapseDelay)
            {
                TriggerCollapse();
                _respawnTimer += Time.deltaTime;

                if(_respawnTimer >= RespawnDelay)
                {
                    ResetObject();
                    _respawnTimer = 0f;
                    _collapseTimer = 0f;

                    _collapseTriggered = false;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player collided with platform");
            if (collision.gameObject.GetComponent<PlayerGround>().GetOnGround())
            {
                //Debug.Log("Player grounded on collapsing platform");
                _collapseTriggered = true;
            }
        }
    }

    public void TriggerCollapse()
    {
        _collider.enabled = false;
        ActiveRender.SetActive(false);
        InactiveRender.SetActive(true);
    }

    public void ResetObject()
    {
        InactiveRender.SetActive(false);
        ActiveRender.SetActive(true);
        _collider.enabled = true;
    }

}
