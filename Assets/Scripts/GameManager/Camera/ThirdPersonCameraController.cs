using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 100f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;

    private CinemachineVirtualCamera _virtualCamera;
    private float _verticalRotation = 0f;
    private float _horizontalRotation = 0f;

    void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();

        Cursor.lockState = CursorLockMode.Locked; // <- blocca il cursore al centro dello schermo

        //Cursor.visible = false;
    }

    void Update()
    {
        HandleCameraRotation();

        if (Input.GetKeyDown(KeyCode.Escape)) // <- sblocca cursore con ESC
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void HandleCameraRotation()
    {
        // input del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // calcola rotazioni
        _horizontalRotation += mouseX;
        _verticalRotation -= mouseY;

        _verticalRotation = Mathf.Clamp(_verticalRotation, minVerticalAngle, maxVerticalAngle); // <- limita la rotazione verticale

        transform.rotation = Quaternion.Euler(_verticalRotation, _horizontalRotation, 0f); // <- applica la rotazione alla camera
    }
}