using Cinemachine;
using PlayerController;
using States;
using UnityEngine;
using Combat;
namespace States
{
    public class PlayerSwordFreeState : PlayerCombatFreeState
    {
        public PlayerSwordFreeState(PlayerStateMachine player, Weapon weapon = Weapon.Sword, bool entryAttack = false, bool autoStateChange = false) : base(player, weapon, entryAttack, autoStateChange)
        {
        }

        public override void Enter()
        {   
            //animationController.SetBoolsCombatFree(weapon, !animationController.IsUnsheathAnimPlaying);
            if (stateMachine.PreviousState != stateMachine.SwordTargetState && stateMachine.PreviousState != stateMachine.AimState && stateMachine.PreviousState != stateMachine.ReturnSwordState && stateMachine.PreviousState != stateMachine.RollState)
            {
                animationController.PlayUnsheathSword();
                animationController.FreeCombat(Weapon.Sword, false);
            }
            else
            {
                animationController.FreeCombat(Weapon.Sword, true);
                if(stateMachine.PreviousState == stateMachine.RollState && stateMachine.RollState.IsAttack)
                {
                    entryAttack = true;
                }
                else
                {
                    entryAttack = false;
                }
            }
            base.Enter();
        }

        protected override void StateTickActions(float deltaTime)
        {
            if(IsAttacking)
            {
                RotateCharacterAttack(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), movement.RotateAfterAttackTime);
                IsAttacking = false;
            }
            if (animationController.IsUnsheathSheathAnimPlaying)
                return;
            //Cinemachine IsBlending doesn't work properly at start
            if(stateMachine.CameraController.IsAimCameraActive) 
            {
                return;
            }
            if (stateMachine.CameraController.IsTransition)
            {
                animationController.SwordFreeMovement(Vector2.zero);
                return;
            }

            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;


            if (movementOn2DAxis.magnitude > 0.06f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
            
            if (IsSprintHold || IsSprint)
            {
                animationController.SprintSetFloats(movementOn2DAxis);
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.CombatSprintSpeed, deltaTime);
            }
            else
            {
                animationController.SwordFreeMovement(movementOn2DAxis);
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.SwordFreeSpeed, deltaTime);
            }
            
            HandleSprintControl();

            if (stateMachine.IsRoll)
                stateMachine.ChangeState(stateMachine.RollState);
        }

        protected override void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(stateMachine.SwordTargetState);
        }

        protected override void HandleSheathEvent()
        {
            if(animationController.IsAttackPlaying) return;
            stateMachine.ChangeState(stateMachine.UnarmedFreeState);
        }

        protected override void HandleOnAimHoldEvent()
        {
            stateMachine.ChangeState(stateMachine.AimState);
        }
    }
}

