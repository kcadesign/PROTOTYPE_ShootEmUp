using UnityEngine;

public class TextController : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    public float rate = 1f;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (_canvasGroup == null)
        {
            Debug.LogError("CanvasGroup component not found on " + gameObject.name);
        }
        _canvasGroup.alpha = 0f; // start with the text invisible
    }

    private void Update()
    {
        _canvasGroup.alpha = Mathf.PingPong(Time.time * rate, 1f);
    }


}
