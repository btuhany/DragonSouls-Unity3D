using Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("FreeLookState")]
    public float FreeLookMaxMovementSpeed = 10f;
    public float _faceDirectionRotationLerpTimeScale = 3f;
    [Header("TargetState")]
    public float TargetMovementSpeed = 4f;
    public float _targetDirectionRotationLerpTimeScale = 2f;
    [Header("Components")]
    [SerializeField] private CharacterController _characterController;
    private Transform _mainCam;
    private void Awake()
    {
        _mainCam = Camera.main.transform;
    }
    public Vector3 CamRelativeMotionVector(Vector2 input2DMovementVector)
    {
        Vector3 normalizedForwardVector = _mainCam.forward * input2DMovementVector.y;

        Vector3 normalizedRightVector = _mainCam.right * input2DMovementVector.x;

        Vector3 normalizedRelativeVector = normalizedForwardVector + normalizedRightVector;
        normalizedRelativeVector.y = 0f;

        return normalizedRelativeVector;
    }
    public Vector3 TargetRelativeMotionVector(Vector3 targetPos)
    {
        Vector3 relativeVector = targetPos - transform.position;
        relativeVector.y = 0f;

        return relativeVector;
    }
    public void LookRotation(Vector3 movementVector, float deltaTime)
    {
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(movementVector),
                deltaTime * _faceDirectionRotationLerpTimeScale
                );
        }
    }
    public void Move(Vector3 motion, float speed, float deltaTime)
    {
        _characterController.Move(motion * speed * deltaTime);
    }
    
}
