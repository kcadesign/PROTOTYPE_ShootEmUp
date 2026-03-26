using Unity.VisualScripting;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    private Rigidbody2D _enemyRigidbody;

    private bool _moveLeft = true;

    private Vector2 _direction = new Vector2(-1, 1);
    public float JumpPower = 1f;

    public float JumpDelay = 4f;
    //private float _randomisedJumpDelay = 0;
    private float _randomStartDelay = 0;
    private float _jumpTimer = 0;

    private void Awake()
    {
        _enemyRigidbody = GetComponent<Rigidbody2D>();
        //_randomisedJumpDelay = Random.Range(JumpDelay + 1, JumpDelay - 1);
        _randomStartDelay = Random.Range(0, 3);
    }

    private void Start()
    {
        InvokeRepeating(nameof(Jump), _randomStartDelay, JumpDelay);
    }

    private void Update()
    {
    }

    private void Jump()
    {
        //_jumpTimer += Time.deltaTime;
        //Debug.Log("Jump timer: " + _jumpTimer);
        //if (_jumpTimer >= JumpDelay)
        //{
            //Debug.Log("Jump timer == Jump delay");
            _enemyRigidbody.linearVelocity = Vector2.up * JumpPower;
        //    _jumpTimer = 0;
        //}
    }
}
