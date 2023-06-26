using PlayerController;
using UnityEngine;
namespace States
{
    public abstract class PlayerCombatTargetState : PlayerCombatState
    {
        private Transform targetTransform;
        private const int lostTargetCancelDelay = 2;
        private float targetRangeControlCounter = lostTargetCancelDelay;

        public PlayerCombatTargetState(PlayerStateMachine player, Weapon weapon, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
        }

        public override void Enter()
        {
            targetRangeControlCounter = 3f;
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
            animationController.TargetMovementBlendTree(inputReader.MovementOn2DAxis);

            if (isSprintHold || isSprint)
            {
                RotateCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), deltaTime);
                MoveCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), movement.TargetRunSpeed, deltaTime);
            }
            else
            {
                RotateCharacter(movement.TargetRelativeMotionVector(targetTransform.position), deltaTime);
                MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
            }
            TargetRangeControl(deltaTime);
            HandleSprintControl();
        }
        private void TargetRangeControl(float deltaTime)
        {
            targetRangeControlCounter -= deltaTime;
            if (targetRangeControlCounter < 0)
            {
                targetRangeControlCounter = lostTargetCancelDelay;
                if (!targetableCheck.IsTargetInRange())
                {
                    stateMachine.ChangeState(stateMachine.FreeLookPlayerState);
                    return;
                }
            }
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
