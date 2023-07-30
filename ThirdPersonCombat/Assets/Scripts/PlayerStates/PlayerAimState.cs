using Inputs;
using PlayerController;
using TMPro;
using UnityEngine;
using Combat;
namespace States
{
    public class PlayerAimState : PlayerBaseState
    {
        private bool _isThrowed = false;
        private float _animationTime = 0f;
        private CombatController _combat;
        private Transform targetTransform;
        public bool IsTargeted = false;
        public PlayerAimState(PlayerStateMachine player) : base(player)
        {
            _combat = player.CombatController;
        }

        public override void Enter()
        {
            if(stateMachine.PreviousState != stateMachine.RollState) 
            {
                IsTargeted = false;
            }
            if (stateMachine.PreviousState == stateMachine.SwordTargetState || (stateMachine.PreviousState == stateMachine.RollState && stateMachine.RollState.IsTargeted))
            {
                if (targetableCheck.TryTransferTarget())
                {
                    targetTransform = targetableCheck.CurrentTargetTransform;
                    IsTargeted = true;
                }
            }
            if (!stateMachine.CameraController.IsTransition)
            {
                if (!(stateMachine.PreviousState == stateMachine.RollState && stateMachine.RollState.IsFastRoll))
                {
                    LookRotationCameraForward();
                    stateMachine.CameraController.AimCamSetVerticalRotation(Camera.main.transform.rotation.eulerAngles.x);
                }
                
            }
            if(!IsTargeted)
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
                    if(IsTargeted)
                        stateMachine.ChangeState(stateMachine.UnarmedTargetState);
                    else
                        stateMachine.ChangeState(stateMachine.UnarmedFreeState);
                    return;
                }

                if (IsTargeted)
                {
                    Vector3 relativeVector = targetTransform.position - Camera.main.transform.position;
                    relativeVector.y = 0f;
                    RotateCharacter(relativeVector, deltaTime);
                }
                else
                {
                    RotateAround(Vector3.up, inputReader.CameraMovementOn2DAxis.x * movement.AimStateCameraHorizontalRotationPower);
                }

                return;
            }

            Vector2 movementVector = inputReader.MovementOn2DAxis;

            if (IsTargeted)
            {
                Vector3 relativeVector = targetTransform.position - Camera.main.transform.position;
                stateMachine.CameraController.SetAimCamTarget(targetTransform, relativeVector);
                relativeVector.y = 0f;
                RotateCharacter(relativeVector, deltaTime);
                MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
                animationController.TargetStateSetFloats(movementVector);
            }
            else
            {
                stateMachine.CameraController.ResetAimCamTarget();
                MoveCharacter(movement.CamRelativeMotionVector(movementVector), movement.AimMovementSpeed, deltaTime);

                //Character horizontal rotation
                RotateAround(Vector3.up, inputReader.CameraMovementOn2DAxis.x * movement.AimStateCameraHorizontalRotationPower);

                //Camera vertical rotation
                stateMachine.CameraController.AimCamRotation(inputReader.CameraMovementOn2DAxis.y * movement.AimStateCameraVerticalRotationPower);

                animationController.TargetStateSetFloats(inputReader.CameraMovementOn2DAxis + movementVector);
            }
            if (stateMachine.IsRoll)
                stateMachine.ChangeState(stateMachine.RollState);
        }

        public override void Exit()
        {
            _isThrowed = false;
            _animationTime = 0f;
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
            if(IsTargeted)
            {
                IsTargeted = false;
                targetTransform = null;
                targetableCheck.ClearTarget();
                _combat.SetAciveCrosshair(true);
            }
            else
            {
                if (!targetableCheck.TrySelectTarget()) return;
                targetTransform = targetableCheck.CurrentTargetTransform;
                IsTargeted = true;
                _combat.SetAciveCrosshair(false);
            }
        }

        protected override void HandleSheathEvent()
        {
        }
        protected override void HandleOnAimHoldCancelEvent()
        {
            if (!_isThrowed)
            {
                if (IsTargeted)
                    stateMachine.ChangeState(stateMachine.SwordTargetState);
                else
                    stateMachine.ChangeState(stateMachine.SwordFreeState);
            }
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

        protected override void HandleOnTargetSelect(Vector2 selectDir)
        {
            if (!IsTargeted) return;
            targetableCheck.ChangeTarget(selectDir);
            targetTransform = targetableCheck.CurrentTargetTransform;
        }
    }
}
