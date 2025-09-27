using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public AxisState xAxis;
    public AxisState yAxis;
    public Transform lookAt;

    void Update()
    {
        HandleCameraRotation();
    }

    void HandleCameraRotation()
    {
        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        lookAt.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, xAxis.Value, 0), 5 * Time.deltaTime);
    }
}