using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum MOVEMENT_TYPE { NORMAL_MOVE, TANK_MOVE } // <- tipo di movimento utilizzabile


    [Header("Movement Settings")]
    [SerializeField] private MOVEMENT_TYPE _movementType = MOVEMENT_TYPE.NORMAL_MOVE;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintMultiplier = 2f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private GroundChecker _groundChecker;
    private float _h;
    private float _v;
    private float _currentSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 5f;
    private bool _j;
    private int _maxJumps = 2;
    private int _jumpCount = 0;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_groundChecker == null) _groundChecker = GetComponentInChildren<GroundChecker>(); // <- se non e`associata, assegno la componente al primo Child
    }

    private void Update()
    {
        OnInput();
    }

    private void FixedUpdate()
    {
        OnMovementType();
        OnJump();
    }

    private void OnInput() // <- per gestire gli input di movimento e salto
    {
        _h = Input.GetAxis("Horizontal");
        _v = Input.GetAxis("Vertical");
        _j = Input.GetButtonDown("Jump");
    }

    private void OnMovementType() // <- per scegliere quale tipo di movimento adottare
    {
        switch (_movementType)
        {
            case MOVEMENT_TYPE.NORMAL_MOVE:
                NormalMove();
                break;

            case MOVEMENT_TYPE.TANK_MOVE:
                TankMove();
                break;
        }
    }

    private void NormalMove() // <- permette di muovere il player normalmente
    {
        if (_h != 0f || _v != 0f) // <- gestisco la fisica al variare di "h" o "v"
        {
            Vector3 direction = new Vector3(_h, 0, _v); // <- creo un vettore direzione

            if (direction.sqrMagnitude > 0.05f)
            {
                direction.Normalize(); // <- quindi la normalizzo

                //transform.forward = direction; // <- ruoto il personaggio nella direzione dove sto andando in modo smooth

                //altro modo per eseguire un cambio di direzione smooth
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion smoothRotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                _rigidbody.MoveRotation(smoothRotation);

                //Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, _rotationSpeed * Time.deltaTime, 0f); // <- utilizzo RotateTowards per eseguire un cambio di direzione smooth
                //transform.rotation = Quaternion.LookRotation(newDirection);

                _rigidbody.MovePosition(_rigidbody.position + direction * (_currentSpeed * Time.deltaTime)); // <- eseguo il movimento tramite MovePosition()
            }
        }
    }

    private void TankMove() // <- permette di muovere il player alla Resident Evil
    {
        if (_h != 0 || _v != 0) // <- gestisco il movimento al variare di "h" o "v"
        {
            float move = _v * _speed * Time.deltaTime;
            float yaw = _h * _rotationSpeed * 10f * Time.deltaTime;

            _rigidbody.MovePosition(_rigidbody.position + transform.forward * move);
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(0f, yaw, 0f));
        }
    }
    private void SpeedMove() // <- permette di correre
    {
        _currentSpeed = Input.GetButton("Debug Multiplier") ? _speed * _sprintMultiplier : _speed; // <- se tengo premuto LeftShift, la velocità raddoppia, altrimenti resta al suo velore originale
                                                                                                   // ^- grazie Jacopo per questa codifica dell'IF :)
    }

    private void OnJump() // <- gestisce il doppio salto
    {
        if (_groundChecker.IsGrounded) _jumpCount = 0; // <- reset contatore dei salti se siamo a terra

        if (_j && _jumpCount < _maxJumps - 1)
        {
            _rigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            _jumpCount++;
        }
    }
}
