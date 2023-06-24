
using UnityEngine;
namespace States
{
    // TODO: Event returns
    public class PlayerFreeLookState : PlayerBaseState
    {
        private bool _isSprintHold = false;
        private bool _isSprint = false;
        public PlayerFreeLookState(PlayerStateMachine player) : base(player)
        {
        }

        public override void Enter()
        {
            animationController.PlayFreeLook();
            inputReader.TargetEvent += HandleOnTargetEvent;
            inputReader.SprintHoldEvent += HandleOnSprintHoldEvent;
            inputReader.SprintHoldCanceledEvent += HandleOnSprintHoldCancelEvent;
            inputReader.SprintEvent += HandleOnSprintEvent;
            inputReader.LightAttackEvent += HandleOnLightAttackEvent;
        }

        public override void Exit()
        {
            inputReader.TargetEvent -= HandleOnTargetEvent;
            inputReader.SprintHoldEvent -= HandleOnSprintHoldEvent;
            inputReader.SprintHoldCanceledEvent -= HandleOnSprintHoldCancelEvent;
            inputReader.SprintEvent -= HandleOnSprintEvent;
            inputReader.LightAttackEvent -= HandleOnLightAttackEvent;
        }

        public override void Tick(float deltaTime)
        {
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;

            HandleMovementAnimation();

            if (_isSprintHold || _isSprint)
            {
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis.normalized), movement.SprintMovementSpeed, deltaTime);
            }
            else
            {
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.FreeLookMaxMovementSpeed, deltaTime);
            }


            if (movementOn2DAxis.magnitude > 0f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }

            HandleSprintControl();
        }

        private void HandleMovementAnimation()
        {
            animationController.FreeLookMovementBlendTree(inputReader.MovementOn2DAxis);
        }
        private void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            player.ChangeState(player.TargetPlayerState);
        }
        private void HandleOnSprintHoldEvent()
        {
            _isSprintHold = true;
            animationController.Sprint(true);
        }
        private void HandleOnSprintHoldCancelEvent()
        {
            _isSprintHold = false;
            if (_isSprint) return;
            animationController.Sprint(false);
        }
        private void HandleOnSprintEvent()
        {
            _isSprint = true;
            animationController.Sprint(true);
        }
        private void HandleSprintControl()
        {
            if (_isSprint && movement.Velocity.sqrMagnitude < 0.1f)
            {
                _isSprint = false;
                if (_isSprintHold) return;
                animationController.Sprint(false);
            }
        }
        private void HandleOnLightAttackEvent()
        {
            player.ChangeState(player.LightAttackState);
        }

    }
}


