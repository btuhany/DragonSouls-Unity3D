using PlayerController;
using UnityEngine;
namespace States
{
    public class PlayerCombatTargetState : PlayerCombatState
    {
        private Transform targetTransform;
        private const int lostTargetCancelDelay = 2;
        private float targetRangeControlCounter = lostTargetCancelDelay;

        public PlayerCombatTargetState(PlayerStateMachine player, Weapon weapon, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
        }

        protected override void StateEnterActions()
        {
            animationController.PlaySetBoolsCombatTargetBlendSetBools();
            targetRangeControlCounter = 3f;
            targetTransform = targetableCheck.CurrentTargetTransform;
            inputReader.TargetEvent += HandleOnTargetEvent;
        }
        protected override void StateExitActions()
        {
            animationController.CancelTargetBools();
            targetTransform = null;
            targetableCheck.ClearTarget();
            inputReader.TargetEvent -= HandleOnTargetEvent;
        }
        protected override void StateTickActions(float deltaTime)
        {
            animationController.TargetMovementBlendTree(inputReader.MovementOn2DAxis);
            RotateCharacter(movement.TargetRelativeMotionVector(targetTransform.position), deltaTime);
            MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
            TargetRangeControl(deltaTime);
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
        private void HandleOnTargetEvent()
        {
            stateMachine.ChangeState(stateMachine.FreeLookPlayerState);
        }

        private Vector3 MotionVectorAroundTarget()
        {
            //Character always looks to target
            Vector3 motion = Vector3.zero;
            motion += transform.right * inputReader.MovementOn2DAxis.x;
            motion += transform.forward * inputReader.MovementOn2DAxis.y;
            return motion;
        }
        private void HandleOnLightAttackEvent()
        {

        }
        private void HandleOnHeavyAttackEvent()
        {

        }
    }

}
