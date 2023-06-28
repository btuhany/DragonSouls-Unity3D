using Inputs;
using PlayerController;
using TMPro;
using UnityEngine;

namespace States
{
    public class PlayerAimState : PlayerBaseState
    {
        private bool _isThrowed = false;
        private float _animationTime = 0f;
        private CombatController _combat;
  
        public PlayerAimState(PlayerStateMachine player) : base(player)
        {
            _combat = player.CombatController;
        }

        public override void Enter()
        {
            //if(stateMachine.PreviousState == stateMachine.SwordFreeState || stateMachine.PreviousState == stateMachine.SwordTargetState)
            animationController.PlayAimSword();
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            Vector2 movementVector = inputReader.MovementOn2DAxis;
            MoveCharacter(movement.CamRelativeMotionVector(movementVector), movement.AimMovementSpeed, deltaTime);

            animationController.TargetMovementBlendTree(inputReader.CameraMovementOn2DAxis + movementVector);

            //Character horizontal rotation
            RotateAround(Vector3.up, inputReader.CameraMovementOn2DAxis.x * movement.AimStateCameraHorizontalRotationPower);

            //Camera vertical rotation
            stateMachine.AimStateFocus.rotation *= Quaternion.AngleAxis(inputReader.CameraMovementOn2DAxis.y * movement.AimStateCameraVerticalRotationPower, Vector3.left);
  
            if (_isThrowed)
            {
                _animationTime += deltaTime;
                if(_animationTime > _combat.ThrowAttack.attackDuration + _combat.ThrowAttack.comboPermissionDelay)
                {
                    stateMachine.ChangeState(stateMachine.UnarmedFreeState);
                }
            }
        }

        public override void Exit()
        {
            _isThrowed = false;
            _animationTime = 0f;
            base.Exit();
        }
        protected override void HandleOnHeavyAttackEvent()
        {
            ThrowSword();
            _isThrowed = true;
        }

        protected override void HandleOnLightAttackEvent()
        {
            ThrowSword();
            _isThrowed = true;
        }

        protected override void HandleOnTargetEvent()
        {
        }

        protected override void HandleSheathEvent()
        {
        }
        protected override void HandleOnAimHoldCancelEvent()
        {
            if(!_isThrowed)
                stateMachine.ChangeState(stateMachine.SwordFreeState);
        }

        private void ThrowSword()
        {
            animationController.PlayAttack(_combat.ThrowAttack.animationName,_combat.ThrowAttack.transitionDuration);
        }

        
    }
}
