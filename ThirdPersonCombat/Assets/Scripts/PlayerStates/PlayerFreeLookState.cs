using Combat;
using UnityEngine;
namespace States
{
    public class PlayerFreeLookState : PlayerBaseState
    {

        public PlayerFreeLookState(PlayerStateMachine player) : base(player)
        {
        }

        public override void Enter()
        {
            animationController.PlaySetFreeLookBlend();
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;

            if ((IsSprintHold || IsSprint) && stateMachine.stamina.UseStamina(movement.SprintStaminaCost))
            {
                animationController.SprintSetFloats(movementOn2DAxis);
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis.normalized), movement.FreeLookSprintMovementSpeed, deltaTime);
            }
            else
            {
                animationController.FreeLookMovementBlendTree(movementOn2DAxis);
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.FreeLookMaxMovementSpeed, deltaTime);
            }


            if (movementOn2DAxis.magnitude > 0f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }

            if (stateMachine.isRoll)
                stateMachine.ChangeState(stateMachine.rollState);
            HandleSprintControl();
        }

        protected override void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(stateMachine.swordTargetState);
        }

        protected override void HandleOnLightAttackEvent()
        {
            if (stateMachine.stamina.UseStamina(movement.LightAttackStaminaCost))
                stateMachine.ChangeState(stateMachine.unarmedFreeTransitionState);
        }

        protected override void HandleOnHeavyAttackEvent()
        {
            if (stateMachine.stamina.UseStamina(movement.HeavyAttackStaminaCost))
                stateMachine.ChangeState(stateMachine.unarmedFreeTransitionState);
        }

        protected override void HandleSheathEvent()
        {
            stateMachine.ChangeState(stateMachine.swordFreeState);
        }

    }
}


