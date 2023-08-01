using States;
using UnityEngine;

public class EnemyMovementController : MonoBehaviour
{
    private CharacterController _characterController;

    public Vector3 Velocity => _characterController.velocity;
    private void Awake()
    {
        _characterController = GetComponentInChildren<CharacterController>();
        GetComponent<EnemyStateMachine>().OnDead += HandleOnDead;
    }
    public Vector3 TargetRelativeMotionVector(Vector3 targetPos)
    {
        Vector3 relativeVector = targetPos - transform.position;
        relativeVector.y = 0f;

        return relativeVector;
    }
    public void LookRotation(Vector3 dir, float lerpTimeScale, float deltaTime)
    {
        if (dir != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(dir),
                deltaTime * lerpTimeScale
                );
        }
    }
    public void LookRotation(Vector3 dir)
    {
        transform.rotation = Quaternion.LookRotation(dir);
    }
    public void Move(Vector3 motion, float speed, float deltaTime)
    {
        _characterController.Move(motion * speed * deltaTime);
    }
    private void HandleOnDead()
    {
        _characterController.enabled = false;
    }
}
