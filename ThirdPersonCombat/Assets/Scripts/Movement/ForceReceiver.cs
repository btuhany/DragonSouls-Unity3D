using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private float _gravityScale = 1;
    [SerializeField] private CharacterController _characterController;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private float _checkRadius;
    [SerializeField] private LayerMask _groundMask;

    private Vector3 verticalVelocity = Vector3.zero;
    private readonly float _gravity = Physics.gravity.y;
    private bool _isGrounded => Physics.CheckSphere(_groundCheckTransform.position, _checkRadius, _groundMask);
    private void Update()
    {
        verticalVelocity.y += _gravity * Time.deltaTime * _gravityScale;

        if (_isGrounded && verticalVelocity.y < 0f)
            verticalVelocity.y = -2f;
        _characterController.Move(verticalVelocity * Time.deltaTime);
    }
 
    
} 
