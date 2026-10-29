using UnityEngine;
using UnityEngine.UIElements;

public class Collapse : MonoBehaviour
{
    private Collider2D _collider;

    [Header("Activator")]
    public bool PlayerActivated = true;

    [Header("Render Visuals")]
    public GameObject ActiveRender;
    public GameObject InactiveRender;

    private Vector3 _originalPosition;
    private Vector3 _position;

    [Header("Unstable State Settings")]
    public float HorizontalFrequency = 1.0f;
    public float HorizontalMagnitude = 1.0f;

    private bool _collapseTriggered = false;
    private float _collapseTimer = 0f;

    [Header("Collapse Settings")]
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

        _originalPosition = transform.position;
        _position = transform.position;
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
            UnstableState();

            if (_collapseTimer >= CollapseDelay)
            {
                ResetPosition();
                TriggerCollapse();
                _respawnTimer += Time.deltaTime;

                if (_respawnTimer >= RespawnDelay)
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
        if (collision.gameObject.CompareTag("Player") && PlayerActivated)
        {
            //Debug.Log("Player collided with platform");
            if (collision.gameObject.GetComponent<PlayerGround>().GetOnGround())
            {
                //Debug.Log("Player grounded on collapsing platform");
                _collapseTriggered = true;
            }
        }
    }

    public void UnstableState()
    {
        // can change the visual here also if needed
        transform.position = _position + new Vector3(Mathf.Sin(Time.time * HorizontalFrequency) * HorizontalMagnitude, 0, 0);

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

    public void ResetPosition()
    {
        transform.position = _originalPosition;
        _position = _originalPosition;
    }
}
