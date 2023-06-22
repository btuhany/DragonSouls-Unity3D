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
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
            MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.FreeLookMovementSpeed, deltaTime);
            animationController.FreeLookMovementAnim(movementOn2DAxis.magnitude);
            
            if (movementOn2DAxis.magnitude > 0.3f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
        }


        private void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(player.TargetPlayerState);
        }
    }
}


