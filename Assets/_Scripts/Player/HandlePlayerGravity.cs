using System;
using UnityEngine;

public class HandlePlayerGravity : MonoBehaviour
{
    public static event Action<bool> OnPlayerDescending;

    private Rigidbody2D _playerRigidbody;
    private PlayerGround _onGround;
    private Jump _playerJump;
    private Dash _playerDash;

    private float _defaultGravityScale;
    public float SpeedLimit;


    private bool _isDescending;

    public float AscendingGravity = 1f;
    public float DescendingGravity = 6.17f;
    private float _jumpGravMultiplier;

    private void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _onGround = GetComponent<PlayerGround>();
        _playerJump = GetComponent<Jump>();
        _playerDash = GetComponent<Dash>();
    }

    private void Start()
    {
        if (_playerRigidbody != null)
        {
            _playerRigidbody.gravityScale = _defaultGravityScale;
        }
    }

    private void Update()
    {
        CheckDescending();
    }

    private void FixedUpdate()
    {
        //CalculateGravity();
        LimitFallSpeed();
    }

    //public void CalculateGravity()
    //{
    //    const float errorThreshold = 0.01f;

    //    if (_playerRigidbody.linearVelocityY > errorThreshold && _playerJump.GetIsPressingJump())
    //    {
    //        _jumpGravMultiplier = _defaultGravityScale * AscendingGravity;
    //    }
    //    else if (_playerRigidbody.linearVelocityY < -errorThreshold)
    //    {
    //        _jumpGravMultiplier = _defaultGravityScale * DescendingGravity;
    //    }
    //    else
    //    {
    //        if (_onGround && Mathf.Abs(_playerRigidbody.linearVelocityY) <= errorThreshold)
    //        {
    //            _jumpGravMultiplier = _defaultGravityScale;
    //        }
    //        else
    //        {
    //            _jumpGravMultiplier = _defaultGravityScale * DescendingGravity;
    //        }
    //    }

    //    _playerRigidbody.gravityScale = _jumpGravMultiplier;
    //}

    private void CheckDescending()
    {
        if (_playerRigidbody.linearVelocityY < 0)
        {
            _isDescending = true;
            OnPlayerDescending?.Invoke(_isDescending);
        }
        else
        {
            _isDescending = false;
            OnPlayerDescending?.Invoke(_isDescending);
        }
    }

    private void LimitFallSpeed()
    {
        if (_playerRigidbody.linearVelocityY < -SpeedLimit)
        {
            _playerRigidbody.linearVelocityY = -SpeedLimit;
        }
    }

    private void SetAscendingGravity()
    {

    }

    private void SetDescendingGravity()
    {

    }

    private void SetDashingGravity()
    {

    }

    public void SetDefaultGravityScale(float gravityScale)
    {
        _playerRigidbody.gravityScale = gravityScale;
    }

    public float GetDefaultGravityScale()
    {
        return _defaultGravityScale;
    }
}
