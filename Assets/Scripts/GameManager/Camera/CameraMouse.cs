using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouse : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _cameraOffset;

    [Range(0.01f, 1f)]
    [SerializeField] private float _smoothness = .5f;
    [SerializeField] private bool _lookAtPlayer = false;
    [SerializeField] private bool _rotateAroundPlayer = true;
    [SerializeField] private float _rotationSpeed = 5f;

    private void Start()
    {
        _cameraOffset = transform.position - _player.position;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (_rotateAroundPlayer)
        {
            Quaternion cameraTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * _rotationSpeed, Vector3.up);
            _cameraOffset = cameraTurnAngle * _cameraOffset;
        }
        Vector3 newPosition = _player.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPosition, _smoothness);

        if (_lookAtPlayer || _rotateAroundPlayer) transform.LookAt(_player);
    }
}