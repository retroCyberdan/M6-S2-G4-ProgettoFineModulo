using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerFSM : MonoBehaviour
{
    private enum MOVEMENT_TYPE { NORMAL_MOVE, TANK_MOVE }

    [Header("Movement Settings")]
    [SerializeField] private MOVEMENT_TYPE _movementType = MOVEMENT_TYPE.NORMAL_MOVE;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintMultiplier = 2f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private GroundChecker _groundChecker;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 5f;
    private int _maxJumps = 2;
    private int _jumpCount = 0;

    [Header("Audio Settings")]
    [SerializeField] private float _footstepInterval = 0.5f;
    private float _lastFootstepTime;

    // inputs
    private float _h;
    private float _v;
    private bool _j;
    private float _currentSpeed;

    // components
    private Rigidbody _rigidbody;
    private StateMachine _stateMachine;

    // properties per gli stati
    public GroundChecker GroundChecker => _groundChecker;
    public bool JumpInput => _j;
    public float HorizontalInput => _h;
    public float VerticalInput => _v;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (_groundChecker == null) _groundChecker = GetComponentInChildren<GroundChecker>();

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _stateMachine = new StateMachine();

        // aggiungi tutti gli stati
        _stateMachine.AddState(PlayerStateType.IDLE, new IdleState(this));
        _stateMachine.AddState(PlayerStateType.MOVING, new MovingState(this));
        _stateMachine.AddState(PlayerStateType.JUMPING, new JumpingState(this));
        _stateMachine.AddState(PlayerStateType.IN_AIR, new InAirState(this));

        _stateMachine.Initialize(PlayerStateType.IDLE); // <- inizializza con stato IDLE
    }

    private void Update()
    {
        HandleInput();
        _stateMachine.UpdateStateMachine();
    }

    private void FixedUpdate()
    {
        _stateMachine.FixedUpdateStateMachine();
    }

    private void HandleInput()
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");
        _j = Input.GetButtonDown("Jump");
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        _currentSpeed = Input.GetButton("Fire3") ? _speed * _sprintMultiplier : _speed;
    }

    #region Public Methods per gli Stati
    public bool HasMovementInput()
    {
        return Mathf.Abs(_h) > 0.1f || Mathf.Abs(_v) > 0.1f;
    }

    public bool CanJump()
    {
        return _jumpCount < _maxJumps - 1;
    }

    public void ResetJumpCount()
    {
        _jumpCount = 0;
    }

    public void PerformJump()
    {
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        _jumpCount++;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayJump(transform.position);
    }

    public void HandleMovement()
    {
        bool isMoving = false;

        switch (_movementType)
        {
            case MOVEMENT_TYPE.NORMAL_MOVE:
                isMoving = NormalMove();
                break;
            case MOVEMENT_TYPE.TANK_MOVE:
                isMoving = TankMove();
                break;
        }

        // riproduce il suono dei passi se il player si sta muovendo e è a terra
        if (isMoving && _groundChecker.IsGrounded && Time.time - _lastFootstepTime > _footstepInterval)
        {
            PlayFootstepSound();
            _lastFootstepTime = Time.time;
        }
    }

    private void PlayFootstepSound()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayFootstep(transform.position);
    }
    #endregion

    #region Movement Methods
    private bool NormalMove()
    {
        if (_h != 0f || _v != 0f)
        {
            Vector3 direction = new Vector3(_h, 0, _v);

            if (direction.sqrMagnitude > 0.05f)
            {
                direction.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion smoothRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                _rigidbody.MoveRotation(smoothRotation);

                _rigidbody.MovePosition(_rigidbody.position + direction * (_currentSpeed * Time.deltaTime));

                return true; // <- indica che il player si sta muovendo
            }
        }
        return false;
    }

    private bool TankMove()
    {
        if (_h != 0 || _v != 0)
        {
            float move = _v * _currentSpeed * Time.deltaTime;
            float yaw = _h * _rotationSpeed * 10f * Time.deltaTime;

            _rigidbody.MovePosition(_rigidbody.position + transform.forward * move);
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0f, yaw, 0f));

            return true; // <- indica che il player si sta muovendo
        }
        return false;
    }
    #endregion

    #region Debug
    public PlayerStateType GetCurrentState()
    {
        return _stateMachine.GetCurrentStateType();
    }

    public string GetStateInfo()
    {
        return _stateMachine.GetStateInfo();
    }
    #endregion
}