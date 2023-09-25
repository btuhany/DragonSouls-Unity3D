using Inputs;
using UnityEngine;
namespace Movement
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField] Transform _humanModelTransform;
        [Header("FreeLookState")]
        public float FreeLookMaxMovementSpeed = 10f;
        public float FaceDirectionRotationLerpTimeScale = 3f;
        public float FreeLookSprintMovementSpeed = 15f;
        [Header("Combat")]
        public float CombatSprintSpeed = 15f;
        public float UnarmedFreeSpeed = 10f;
        public float SwordFreeSpeed = 10f;
        public float RotateAfterAttackTime = 0.2f;
        [Header("TargetState")]
        public float TargetMovementSpeed = 4f;
        public float TargetDirectionRotationLerpTimeScale = 2f;
        public float TargetRunSpeed = 15f;
        [Header("Aim")]
        public float AimMovementSpeed = 3f;
        public float ReturnSwordMovementSpeed = 3f;
        public float ReturnSwordRunMovementSpeed = 6f;
        [Header("Config")]
        public float AimStateCameraHorizontalRotationPower = 0.04f;
        public float AimStateCameraVerticalRotationPower = 5f;
        public float RollStateRotateTime = 1f;
        public float RollDuration = 0.6f;
        public float RollDistance = 4f;
        public float FastRollDuration = 0.3f;
        public float FastRollDistance = 2.5f;
        [Header("StaminaCosts")]
        public float RollStaminaCost = 30f;
        public float SprintStaminaCost = 1f;
        public float LightAttackStaminaCost = 40f;
        public float HeavyAttackStaminaCost = 60f;
        public float SwordThrowStaminaCost = 35f;
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;

        public Vector3 Velocity => _characterController.velocity;

        public CharacterController CharacterController { get => _characterController; set => _characterController = value; }

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
        
        public void RotateHumanModel(float angle)
        {
            _humanModelTransform.localRotation = Quaternion.Euler(0, angle, 0);
        }
    }

}

