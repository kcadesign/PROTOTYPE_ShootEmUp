using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Jump _playerJump;
    public Rigidbody2D _playerRigidbody;
    public GameObject GhostPrefab;

    public bool MakeGhost = false;
    public float GhostFrequency;
    private float _ghostDelaySeconds;
    public float GhostDecayTime = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
        _ghostDelaySeconds = GhostFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerJump.GetIsAirJumping() && _playerRigidbody.linearVelocityY < 0) return;
        if (MakeGhost && _playerJump.GetIsAirJumping() && _playerRigidbody.linearVelocityY > 0)
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
