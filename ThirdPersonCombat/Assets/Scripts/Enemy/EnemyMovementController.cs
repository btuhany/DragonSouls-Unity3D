using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private CharacterController _characterController;

    public Vector3 Velocity => _characterController.velocity;
    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    public Vector3 TargetRelativeMotionVector(Vector3 targetPos)
    {
        Vector3 relativeVector = targetPos - transform.position;
        relativeVector.y = 0f;

        return relativeVector;
    }
    public void LookRotation(Vector3 movementVector, float lerpTimeScale, float deltaTime)
    {
        if (movementVector != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(movementVector),
                deltaTime * lerpTimeScale
                );
        }
    }
    public void LookRotationAround(Vector3 rotation, float value)
    {
        transform.rotation *= Quaternion.AngleAxis(value, rotation);
    }
    public void Move(Vector3 motion, float speed, float deltaTime)
    {
        _characterController.Move(motion * speed * deltaTime);
    }
}
