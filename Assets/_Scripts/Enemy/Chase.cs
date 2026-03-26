using UnityEngine;
using UnityEngine.EventSystems;

public class Chase : MonoBehaviour
{
    private GameObject _player;
    public float DistanceOffset = -3f;
    public float FollowSpeed = 2f;
    public float CreepSpeed = 1f;

    private void Awake()
    {
        // find the player in the scene by searching for the "Player" tag
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        // Lerp towards the player's position but only use the y axis and add an offset to it so the enemy is slightly below the player.
        // Do not allow the enemy to move downward: only allow upward movement by clamping the target y.
        if (_player != null)
        {
            float desiredY = _player.transform.position.y + DistanceOffset;
            // Prevent downward movement: choose the higher of current Y position and desired Y position
            float targetY = Mathf.Max(transform.position.y, desiredY);

            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * FollowSpeed);
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player GameObject has the tag 'Player'.");
        }

        gameObject.transform.Translate(Vector2.up * CreepSpeed * Time.deltaTime);
    }
}
