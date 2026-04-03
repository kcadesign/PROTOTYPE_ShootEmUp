using UnityEngine;

public class Respawn : MonoBehaviour
{
    private GameObject[] _children;
    private float[] _respawnTimers;
    public float RespawnDelay = 3f;

    private void Start()
    {
        // Collect direct child GameObjects (works for inactive children as well)
        int count = transform.childCount;
        _children = new GameObject[count];
        _respawnTimers = new float[count];

        for (int i = 0; i < count; i++)
        {
            Transform childTransform = transform.GetChild(i);
            _children[i] = childTransform != null ? childTransform.gameObject : null;
            _respawnTimers[i] = 0f;
        }
    }

    private void Update()
    {
        if (_children == null || _children.Length == 0)
            return;

        // Process each child independently so each has its own respawn timer
        for (int i = 0; i < _children.Length; i++)
        {
            GameObject child = _children[i];

            if (child == null)
            {
                _respawnTimers[i] = 0f;
                continue;
            }

            if (!child.activeSelf)
            {
                // Increment this child's timer
                _respawnTimers[i] += Time.deltaTime;

                if (_respawnTimers[i] >= RespawnDelay)
                {
                    child.SetActive(true);
                    _respawnTimers[i] = 0f;
                }
            }
            else
            {
                // Reset timer while active so it starts fresh on next deactivation
                _respawnTimers[i] = 0f;
            }
        }
    }
}
