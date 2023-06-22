using PlayerControllers;
using System;
using UnityEngine;
namespace States
{
    public class PlayerTargetState : PlayerBaseState
    {
        private Transform targetTransform;
        private const int lostTargetCancelDelay = 2;
        private float targetRangeControlCounter = lostTargetCancelDelay;
        public PlayerTargetState(PlayerControl player) : base(player)
        {

        }

        public override void Enter()
        {
            targetRangeControlCounter = 3f;
            animationController.PlayTarget();
            targetTransform = targetableCheck.CurrentTargetTransform;
            inputReader.TargetEvent += HandleOnTargetEvent;
        }

        public override void Exit()
        {
            targetTransform = null;
            targetableCheck.ClearTarget();
            inputReader.TargetEvent -= HandleOnTargetEvent;
        }

        public override void Tick(float deltaTime)
        {
            
            RotateCharacter(movement.TargetRelativeMotionVector(targetTransform.position),deltaTime);
            MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
            //TargetRangeControl(deltaTime);
        }
       
        private void TargetRangeControl(float deltaTime)
        {
            targetRangeControlCounter -= deltaTime;
            if(targetRangeControlCounter < 0)
            {
                targetRangeControlCounter = lostTargetCancelDelay;
                if (!targetableCheck.IsTargetInRange())
                {
                    stateMachine.ChangeState(player.FreeLookPlayerState);
                    return;
                }
            }

        }
        private void HandleOnTargetEvent()
        {
            stateMachine.ChangeState(player.FreeLookPlayerState);
        }

        //Character always on looking to target
        private Vector3 MotionVectorAroundTarget()
        {
            Vector3 motion = Vector3.zero;
            motion  += transform.right * inputReader.MovementOn2DAxis.x;
            motion  += transform.forward * inputReader.MovementOn2DAxis.y;
            return motion;
        }
    }
}
