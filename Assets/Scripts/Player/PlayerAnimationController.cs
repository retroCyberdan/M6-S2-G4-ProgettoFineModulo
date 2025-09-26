using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float _walkThreshold = 0.1f;
    [SerializeField] private float _runThreshold = 3f;
    [SerializeField] private float _animationSmoothTime = 0.1f;

    [Header("Components Settings")]
    [SerializeField] private GroundChecker _groundChecker;

    private Animator _animator;

    private readonly int _speedHash = Animator.StringToHash("speed");
    private readonly int _jumpHash = Animator.StringToHash("isJumping");
    private readonly int _isGroundedHash = Animator.StringToHash("isGrounded");

    private float _currentAnimSpeed;
    private bool _wasGrounded = true;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_groundChecker == null) _groundChecker = GetComponentInChildren<GroundChecker>();
    }

    void Update()
    {
        UpdateMovementAnimation();
        UpdateJumpAnimation();
        UpdateGroundedState();
    }

    private void UpdateMovementAnimation()
    {
        float targetSpeed = CalculateAnimationSpeed();

        _currentAnimSpeed = Mathf.MoveTowards(_currentAnimSpeed, targetSpeed, Time.deltaTime / _animationSmoothTime);

        _animator.SetFloat(_speedHash, _currentAnimSpeed);
    }

    private float CalculateAnimationSpeed()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float inputMagnitude = Mathf.Sqrt(h * h + v * v);
        inputMagnitude = Mathf.Clamp01(inputMagnitude);

        bool isSprinting = Input.GetButton("Fire3") || Input.GetKey(KeyCode.LeftShift);

        if (inputMagnitude < _walkThreshold)
        {
            return 0f; // <- idle
        }
        else if (isSprinting && inputMagnitude > 0.5f)
        {
            return _runThreshold; // <- run
        }
        else
        {
            return Mathf.Lerp(_walkThreshold, _runThreshold * 0.6f, inputMagnitude); // <- walk
        }
    }

    private void UpdateJumpAnimation()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded()) _animator.SetTrigger(_jumpHash);
    }

    private void UpdateGroundedState()
    {
        bool isGrounded = IsGrounded();

        _animator.SetBool(_isGroundedHash, isGrounded);
    }

    private bool IsGrounded()
    {
        if (_groundChecker != null) return _groundChecker.IsGrounded;

        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    #region Public Methods
    public void TriggerJump()
    {
        _animator.SetTrigger(_jumpHash);
    }

    public float GetCurrentAnimationSpeed()
    {
        return _currentAnimSpeed;
    }

    public void SetAnimationSpeed(float speed)
    {
        _currentAnimSpeed = speed;
        _animator.SetFloat(_speedHash, speed);
    }
    #endregion
}