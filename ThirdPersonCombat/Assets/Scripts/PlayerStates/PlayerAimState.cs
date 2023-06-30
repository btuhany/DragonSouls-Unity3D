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
        private Transform targetTransform;
        bool _isTargeted = false;
        public PlayerAimState(PlayerStateMachine player) : base(player)
        {
            _combat = player.CombatController;
        }

        public override void Enter()
        {
            stateMachine.AimStateFocus.localRotation = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.x, Vector3.right);
            LookRotationCameraForward();
            _combat.SetAciveCrosshair(true);
            animationController.PlayAimSword();
            base.Enter();
        }

        public override void Tick(float deltaTime)
        {
            if (_isThrowed)
            {
                _animationTime += deltaTime;
                if (_animationTime > _combat.ThrowAttack.attackDuration + _combat.ThrowAttack.comboPermissionDelay)
                {
                    stateMachine.ChangeState(stateMachine.UnarmedFreeState);
                }
                return;
            }

            Vector2 movementVector = inputReader.MovementOn2DAxis;

            if (_isTargeted)
            {
                if (isSprintHold || isSprint)
                {
                    RotateCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), deltaTime);
                    MoveCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), movement.TargetRunSpeed, deltaTime);
                }
                else
                {
                    Vector3 relativeVector = targetTransform.position - Camera.main.transform.position;
                    relativeVector.y = 0f;
                    RotateCharacter(relativeVector, deltaTime);
                    MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
                }
            }
            else
            {
                MoveCharacter(movement.CamRelativeMotionVector(movementVector), movement.AimMovementSpeed, deltaTime);

                //Character horizontal rotation
                RotateAround(Vector3.up, inputReader.CameraMovementOn2DAxis.x * movement.AimStateCameraHorizontalRotationPower);

                //Camera vertical rotation
                stateMachine.AimStateFocus.rotation *= Quaternion.AngleAxis(inputReader.CameraMovementOn2DAxis.y * movement.AimStateCameraVerticalRotationPower, Vector3.left);
            }

            animationController.TargetStateSetFloats(inputReader.CameraMovementOn2DAxis + movementVector);

           
        }

        public override void Exit()
        {
            _isThrowed = false;
            _animationTime = 0f;
            _isTargeted = false;
            targetTransform = null;
            targetableCheck.ClearTarget();
            _combat.SetAciveCrosshair(false);
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
            if(_isTargeted)
            {
                _isTargeted = false;
                targetTransform = null;
                targetableCheck.ClearTarget();
            }
            else
            {
                if (!targetableCheck.TrySelectTarget()) return;
                targetTransform = targetableCheck.CurrentTargetTransform;
                _isTargeted = true;
            }
        }

        protected override void HandleSheathEvent()
        {
        }
        protected override void HandleOnAimHoldCancelEvent()
        {
            if (!_isThrowed)
                stateMachine.ChangeState(stateMachine.SwordFreeState);
        }

        private void ThrowSword()
        {
            animationController.PlayAttack(_combat.ThrowAttack.animationName,_combat.ThrowAttack.transitionDuration);
        }

        private Vector3 MotionVectorAroundTarget()
        {
            //Character always looks to target
            Vector3 motion = Vector3.zero;
            motion += transform.right * inputReader.MovementOn2DAxis.x;
            motion += transform.forward * inputReader.MovementOn2DAxis.y;
            return motion;
        }

        private void LookRotationCameraForward()
        {
            Vector3 pos = transform.position - Camera.main.transform.position;
            pos.y = 0f;
            transform.rotation = Quaternion.LookRotation(pos, Vector3.up);
        }
    }
}
