using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public enum GROUND_CHECK_TYPE {  RAYCAST, CHECK_SPHERE }

    [Header("Ground Checker Settings")]
    [SerializeField] private float _maxDistance = 0.1f;
    [SerializeField] private LayerMask _whatIsGround; // <- per selezionare il tipo di layer
    [SerializeField] private GROUND_CHECK_TYPE _groundCheckType = GROUND_CHECK_TYPE.CHECK_SPHERE; // <- assegno il valore di default

    public bool IsGrounded { get; private set; } // <- creo una property accessibile in lettura ma non in scrittura
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        switch(_groundCheckType)
        {
            case GROUND_CHECK_TYPE.RAYCAST:
                Gizmos.DrawLine(transform.position, transform.position - Vector3.up * _maxDistance);
                break;
            case GROUND_CHECK_TYPE.CHECK_SPHERE:
                Gizmos.DrawWireSphere(transform.position, _maxDistance);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(_groundCheckType)
        {
            case GROUND_CHECK_TYPE.RAYCAST :
                IsGrounded = Physics.Raycast(transform.position, -Vector3.up, _maxDistance);
                break;
            case GROUND_CHECK_TYPE.CHECK_SPHERE :
                IsGrounded = Physics.CheckSphere(transform.position, _maxDistance);
                break;
        }
    }
}