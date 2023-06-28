using Inputs;
using UnityEngine;
namespace Movement
{
    public class MovementController : MonoBehaviour
    {
        [Header("FreeLookState")]
        public float FreeLookMaxMovementSpeed = 10f;
        public float FaceDirectionRotationLerpTimeScale = 3f;
        public float FreeLookSprintMovementSpeed = 15f;
        [Header("Combat")]
        public float CombatSprintSpeed = 15f;
        public float UnarmedFreeSpeed = 10f;
        public float SwordFreeSpeed = 10f;
        [Header("TargetState")]
        public float TargetMovementSpeed = 4f;
        public float TargetDirectionRotationLerpTimeScale = 2f;
        public float TargetRunSpeed = 15f;
        [Header("Aim")]
        public float AimMovementSpeed = 3f;
        [Header("Config")]
        public float AimStateCameraHorizontalRotationPower = 0.04f;
        public float AimStateCameraVerticalRotationPower = 5f;
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;

        public Vector3 Velocity => _characterController.velocity;
        private Transform _mainCam;
        private void Awake()
        {
            _mainCam = Camera.main.transform;
        }
        public Vector3 CamRelativeMotionVector(Vector2 input2DMovementVector)
        {
            Vector3 forwardVector = _mainCam.forward * input2DMovementVector.y;
            Vector3 rightVector = _mainCam.right * input2DMovementVector.x;

            Vector3 relativeVector = forwardVector + rightVector;
            relativeVector.y = 0f;

            return relativeVector;
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
                    deltaTime * FaceDirectionRotationLerpTimeScale
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

}

