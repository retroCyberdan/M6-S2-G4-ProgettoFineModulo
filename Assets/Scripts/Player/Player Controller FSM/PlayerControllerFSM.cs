using UnityEngine;
using UnityEngine.AI;

public class PlayerControllerFSM : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintMultiplier = 2f;
    [SerializeField] private GroundChecker _groundChecker;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 5f;
    private bool _hasJumped = false;

    [Header("Audio Settings")]
    [SerializeField] private float _footstepInterval = 0.5f;
    [SerializeField] private float _sprintFootstepMultiplier = 1.8f; // <- moltiplicatore per la velocità dei passi durante la corsa
    private float _lastFootstepTime;

    #region Inputs

    private float _h;
    private float _v;
    private bool _j;
    private float _currentSpeed;

    #endregion

    #region Components

    private Rigidbody _rigidbody;
    private StateMachine _stateMachine;

    #endregion

    #region Properties

    public GroundChecker GroundChecker => _groundChecker;
    public bool JumpInput => _j;
    public float HorizontalInput => _h;
    public float VerticalInput => _v;

    #endregion

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

        _stateMachine.Initialize(PlayerStateType.IDLE); // <- inizializza con stato IDLE
    }

    private void Update()
    {
        HandleInput();

        HandleJumpReset(); // <- gestisce il reset del salto

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

    private void HandleJumpReset()
    {
        if (_groundChecker.IsGrounded) _hasJumped = false; // <- se è a terra, resetta il flag del salto
    }

    private void UpdateSpeed()
    {
        _currentSpeed = Input.GetButton("Fire3") ? _speed * _sprintMultiplier : _speed;
    }

    #region Public Methods for States
    public bool HasMovementInput()
    {
        return Mathf.Abs(_h) > 0.1f || Mathf.Abs(_v) > 0.1f;
    }

    public bool CanJump()
    {
        return _groundChecker.IsGrounded && !_hasJumped; // <- può saltare solo se è a terra e non ha già saltato
    }

    public void PerformJump()
    {
        _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        _hasJumped = true;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayJump(transform.position);

        Debug.Log("JUMP PERFORMED!");
    }

    public void HandleMovement()
    {
        bool isMoving = TankMove();

        if (isMoving && _groundChecker.IsGrounded)
        {
            // calcola l'intervallo dei passi in base alla velocità
            float currentFootstepInterval = GetCurrentFootstepInterval();

            if (Time.time - _lastFootstepTime > currentFootstepInterval)
            {
                PlayFootstepSound();
                _lastFootstepTime = Time.time;
            }
        }
    }

    private float GetCurrentFootstepInterval()
    {
        // se sta correndo (Fire3 premuto), riduci l'intervallo dei passi
        bool isSprinting = Input.GetButton("Fire3");
        return isSprinting ? _footstepInterval / _sprintFootstepMultiplier : _footstepInterval;
    }

    private void PlayFootstepSound()
    {
        if (AudioManager.Instance != null) AudioManager.Instance.PlayFootstep(transform.position);
    }
    #endregion

    #region Movement Method
    private bool TankMove()
    {
        if (_h != 0 || _v != 0)
        {
            // calcola le direzioni del movimento basate sul transform del player
            Vector3 forward = transform.forward * _v;  // <- su/giù = avanti/indietro
            Vector3 right = transform.right * _h;      // <- sinistra/destra = laterale

            Vector3 moveDirection = forward + right; // <- combina i movimenti

            if (moveDirection.sqrMagnitude > 0.05f)
            {
                moveDirection.Normalize();

                _rigidbody.MovePosition(_rigidbody.position + moveDirection * (_currentSpeed * Time.deltaTime)); // <- muovi il player

                return true;
            }
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