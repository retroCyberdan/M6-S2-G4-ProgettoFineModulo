using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private string _forwardName = "forward";
    [SerializeField] private string _vSpeedName = "vSpeed";
    [SerializeField] private string _jumpName = "jump";
    [SerializeField] private string _isGroundedName = "isGrounded";
    [SerializeField] private float _forwardRange = 2;
    private Animator _animator;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        //_animator.SetFloat(_forwardName, v * 2);
        //_animator.SetFloat(_vSpeedName, _rigidbody.velocity.y);
    }
}