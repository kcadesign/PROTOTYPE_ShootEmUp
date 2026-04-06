using UnityEngine;

public class Respawn : MonoBehaviour
{
    private GameObject[] _children;
    private float[] _timers;
    private bool[] _staggered;
    public float RespawnDelay = 3f;
    public bool IsTimedDespawnRespawn = false;
    public float TimedOffset = 0.5f;

    private void Start()
    {
        int childCount = transform.childCount;
        _children = new GameObject[childCount];
        _timers = new float[childCount];
        _staggered = new bool[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform childTransform = transform.GetChild(i);
            _children[i] = childTransform != null ? childTransform.gameObject : null;
            _timers[i] = 0f;
            _staggered[i] = false;
        }
    }

    private void Update()
    {
        if (_children == null || _children.Length == 0)
            return;

        if (!IsTimedDespawnRespawn)
        {
            for (int i = 0; i < _children.Length; i++)
            {
                GameObject child = _children[i];

                if (child == null)
                {
                    _timers[i] = 0f;
                    _staggered[i] = false;
                    continue;
                }

                if (!child.activeSelf)
                {
                    _timers[i] += Time.deltaTime;

                    if (_timers[i] >= RespawnDelay)
                    {
                        child.SetActive(true);
                        _timers[i] = 0f;
                    }
                }
                else
                {
                    _timers[i] = 0f;
                }
            }
        }
        else // IsTimedDespawnRespawn == true
        {
            for (int i = 0; i < _children.Length; i++)
            {
                GameObject child = _children[i];

                if (child == null)
                {
                    _timers[i] = 0f;
                    _staggered[i] = false;
                    continue;
                }

                // Use staggered offset only for the first deactivation; afterwards use regular RespawnDelay
                float offsetDelay = _staggered[i] ? RespawnDelay : (RespawnDelay + (TimedOffset * i));

                if (child.activeSelf)
                {
                    _timers[i] += Time.deltaTime;

                    if (_timers[i] >= offsetDelay)
                    {
                        child.SetActive(false);
                        _timers[i] = 0f;
                        _staggered[i] = true; // mark that the initial stagger has occurred
                    }
                }
                else // child is inactive
                {
                    _timers[i] += Time.deltaTime;

                    if (_timers[i] >= RespawnDelay)
                    {
                        child.SetActive(true);
                        _timers[i] = 0f;
                    }
                }
            }
        }
    }
}
