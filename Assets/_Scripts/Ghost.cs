using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Jump _playerJump;
    public Dash _playerDash;
    public Grapple _playerGrapple;
    public Rigidbody2D _playerRigidbody;
    public GameObject GhostPrefab;

    public bool MakeGhost = false;
    public float GhostFrequency;
    private float _ghostDelaySeconds;
    public float GhostDecayTime = 0.5f;

    void Start()
    {
        _ghostDelaySeconds = GhostFrequency;
    }

    void Update()
    {
        if (_playerJump.GetIsAirJumping() && _playerRigidbody.linearVelocityY > 0)
        {
            InstantiateGhost();
        }
        else if (_playerDash.GetIsDashing())
        {
            InstantiateGhost();
        }
        else if (_playerGrapple.GetIsGrappling())
        {
            InstantiateGhost();
        }
    }

    private void InstantiateGhost()
    {
        if (MakeGhost)
        {
            if (_ghostDelaySeconds > 0)
            {
                _ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                //Generate ghost
                GameObject currentGhost = Instantiate(GhostPrefab, transform.position, transform.rotation);
                Mesh currentSprite = GetComponent<MeshFilter>().mesh;
                currentGhost.GetComponent<MeshFilter>().mesh = currentSprite;
                _ghostDelaySeconds = GhostFrequency;
                Destroy(currentGhost, GhostDecayTime);
            }
        }
    }
}
