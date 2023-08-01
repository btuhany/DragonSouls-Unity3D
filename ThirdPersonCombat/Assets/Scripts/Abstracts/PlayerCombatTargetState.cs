using UnityEngine;
using Combat;
namespace States
{
    public abstract class PlayerCombatTargetState : PlayerCombatState
    {
        private Transform targetTransform;

        public PlayerCombatTargetState(PlayerStateMachine player, Weapon weapon, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
        }

        public override void Enter()
        {
            targetTransform = targetableCheck.CurrentTargetTransform;
            base.Enter();
        }
        public override void Exit()
        {
            animationController.ResetCombatBools();
            targetTransform = null;
            targetableCheck.ClearTarget();
            base.Exit();
        }

        protected override void StateTickActions(float deltaTime)
        {
            if ((IsSprintHold || IsSprint) && stateMachine.Stamina.UseStamina(movement.SprintStaminaCost))
            {
                RotateCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), deltaTime);
                MoveCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis.normalized), movement.TargetRunSpeed, deltaTime);
                animationController.SprintSetFloats(inputReader.MovementOn2DAxis);
            }
            else
            {
                animationController.TargetStateSetFloats(inputReader.MovementOn2DAxis);
                RotateCharacter(movement.TargetRelativeMotionVector(targetTransform.position), deltaTime);
                MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
            }
            HandleSprintControl();
            if (stateMachine.IsRoll)
                stateMachine.ChangeState(stateMachine.RollState);
        }

        private Vector3 MotionVectorAroundTarget()
        {
            //Character always looks to target
            Vector3 motion = Vector3.zero;
            motion += transform.right * inputReader.MovementOn2DAxis.x;
            motion += transform.forward * inputReader.MovementOn2DAxis.y;
            return motion;
        }

    }

}
