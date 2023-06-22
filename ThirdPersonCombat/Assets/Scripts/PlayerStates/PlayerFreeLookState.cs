using PlayerControllers;
using UnityEngine;
namespace States
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        public PlayerFreeLookState(PlayerControl player) : base(player)
        {
        }

        public override void Enter()
        {
            animationController.PlayFreeLook();
            inputReader.TargetEvent += HandleOnTargetEvent;
        }

        public override void Exit()
        {
            inputReader.TargetEvent -= HandleOnTargetEvent;
        }

        public override void Tick(float deltaTime)
        {
            characterController.Move(CamRelativeMovementVector() * player.MovementSpeed * deltaTime);
            FaceDirectionRotationHandler(deltaTime);
            animationController.FreeLookMovementAnim(inputReader.MovementValue.magnitude);
        }
        private void FaceDirectionRotationHandler(float deltaTime)
        {
            Vector3 movementVector = CamRelativeMovementVector();
            if (movementVector != Vector3.zero)
            {
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(movementVector),
                    deltaTime * player.FaceDirectionRotationLerpTimeScale
                    );
            }
        }
        private Vector3 CamRelativeMovementVector()
        {
            Vector3 normalizedForwardVector = mainCamTransform.forward * inputReader.MovementValue.y;
            normalizedForwardVector.y = 0f;

            Vector3 normalizedRightVector = mainCamTransform.right * inputReader.MovementValue.x;
            normalizedRightVector.y = 0f;
            

            return normalizedForwardVector + normalizedRightVector;
        }
        private void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(player.TargetPlayerState);
        }
    }
}


