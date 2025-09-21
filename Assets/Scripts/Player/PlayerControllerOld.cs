using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControllerOld : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintMultiplier = 2f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private GroundChecker _groundChecker;
    private Rigidbody _rb;
    private float _currentSpeed;
    private int _jumpCount = 0;
    private int _maxJumps = 2;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_groundChecker == null) _groundChecker = GetComponentInChildren<GroundChecker>(); // <- se non e`associata, assegno la componente al primo Child
    }

    private void Update()
    {
        if (_groundChecker.IsGrounded) _jumpCount = 0; // <- reset contatore dei salti se siamo a terra

        if (Input.GetButtonDown("Jump") && _jumpCount < _maxJumps)
        {
            _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
            _jumpCount++;
        }

        _currentSpeed = Input.GetButton("Debug Multiplier") ? _speed * _sprintMultiplier : _speed; // <- se tengo premuto LeftShift, la velocità raddoppia, altrimenti resta al suo velore originale
                                                                                                   // ^- grazie Jacopo per questa codifica dell'IF :)
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // acquisisco gli input
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (h != 0f || v != 0f) // <- gestisco la fisica al variare di "h" o "v"
        {
            Vector3 direction = new Vector3(h, 0, v); // <- creo un vettore direzione

            if (direction.sqrMagnitude > 0.05f)
            {
                direction.Normalize(); // <- quindi la normalizzo

                //transform.forward = direction; // <- ruoto il personaggio nella direzione dove sto andando in modo smooth

                //altro modo per eseguire un cambio di direzione smooth
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                Quaternion smoothRotation = Quaternion.Slerp(_rb.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
                _rb.MoveRotation(smoothRotation);

                //Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, _rotationSpeed * Time.deltaTime, 0f); // <- utilizzo RotateTowards per eseguire un cambio di direzione smooth
                //transform.rotation = Quaternion.LookRotation(newDirection);

                _rb.MovePosition(_rb.position + direction * (_currentSpeed * Time.deltaTime)); // <- eseguo il movimento tramite MovePosition()
            }
        }
    }
}
