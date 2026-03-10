using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlam : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction _slam;

    private Rigidbody2D _rigidbody;
    private PlayerGround _ground;
    //private PlayerJump _playerJump;
    public Animator PlayerAnimator;

    private bool _desiredSlam;
    private bool _isSlamming = false;
    public float SlamForce = 10f;

    [Header("Ground Settings")]
    private bool _onGround = false;

    private void Awake()
    {
        _slam = InputActions.FindAction("Jump");
        _rigidbody = GetComponent<Rigidbody2D>();
        _ground = GetComponent<PlayerGround>();
    }

    private void OnEnable()
    {
        if (!InputActions.FindActionMap("Player").enabled)
        {
            InputActions.FindActionMap("Player").Enable();
        }
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Update()
    {
        _onGround = _ground.GetOnGround();
        //_currentlyJumping = _playerJump.CurrentlyJumping;
        if (_slam.WasPressedThisFrame() && !_onGround)
        {
            _desiredSlam = true;
        }
        else if (_slam.WasReleasedThisFrame() || _onGround)
        {
            _desiredSlam = false;
            _isSlamming = false;
        }
    }

    private void FixedUpdate()
    {
        if (_desiredSlam)
        {
            _isSlamming = true;
            Slam();
            _desiredSlam = false;
        }
    }

    private void Slam()
    {
        _rigidbody.AddForceY(-SlamForce, ForceMode2D.Impulse);

        // play slam animation
        PlayerAnimator.SetTrigger("Slam");

    }

    public bool GetIsSlamming()
    {
        return _isSlamming;
    }
}
