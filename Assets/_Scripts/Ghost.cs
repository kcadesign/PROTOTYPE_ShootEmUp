using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Jump _playerJump;
    public Rigidbody2D _playerRigidbody;
    public GameObject GhostPrefab;

    public bool makeGhost = false;
    public float imageDelay;
    private float ghostDelaySeconds;


    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySeconds = imageDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_playerJump.GetAirJumping() && _playerRigidbody.linearVelocityY < 0) return;
        if (makeGhost && _playerJump.GetAirJumping() && _playerRigidbody.linearVelocityY > 0)
        {
            if (ghostDelaySeconds > 0)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                //Generate ghost
                GameObject currentGhost = Instantiate(GhostPrefab, transform.position, transform.rotation);
                Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
                currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
                ghostDelaySeconds = imageDelay;
                Destroy(currentGhost, 0.5f);
            }
        }

    }
}
