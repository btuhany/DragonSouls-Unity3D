using PlayerControllers;
using System;
using UnityEngine;
namespace States
{
    public class PlayerTargetState : PlayerBaseState
    {
        private Transform targetTransform;
        private float targetRangeControlCounter = 3;
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
            TargetRangeControl(deltaTime);
        }
        //private void FaceDirectionRotationHandler(float deltaTime)
        //{
        //    transform.rotation = Quaternion.Lerp(
        //        transform.rotation,
        //        Quaternion.LookRotation(targetableCheck.CurrentTargetTransform.position),
        //        deltaTime * player.TargetDirectionRotationLerpTimeScale
        //        );
        //}
        private void TargetRangeControl(float deltaTime)
        {
            targetRangeControlCounter -= deltaTime;
            if(targetRangeControlCounter < 0)
            {
                targetRangeControlCounter = 3;
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
    }
}
