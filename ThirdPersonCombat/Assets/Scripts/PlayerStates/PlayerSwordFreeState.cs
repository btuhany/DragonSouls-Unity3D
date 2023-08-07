using Cinemachine;
using PlayerController;
using States;
using UnityEngine;
using Combat;
namespace States
{
    public class PlayerSwordFreeState : PlayerCombatFreeState
    {
        bool isSheating = false;
        public PlayerSwordFreeState(PlayerStateMachine player, Weapon weapon = Weapon.Sword, bool entryAttack = false, bool autoStateChange = false) : base(player, weapon, entryAttack, autoStateChange)
        {
        }

        public override void Enter()
        {
            isSheating = false;
            //animationController.SetBoolsCombatFree(weapon, !animationController.IsUnsheathAnimPlaying);
            if (stateMachine.PreviousState != stateMachine.swordTargetState && stateMachine.PreviousState != stateMachine.aimState && stateMachine.PreviousState != stateMachine.returnSwordState && stateMachine.PreviousState != stateMachine.rollState)
            {
                animationController.PlayUnsheathSword();
                animationController.FreeCombat(Weapon.Sword, false);
            }
            else
            {
                animationController.FreeCombat(Weapon.Sword, true);
                if(stateMachine.PreviousState == stateMachine.rollState && stateMachine.rollState.IsAttack)
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
            if(isSheating)
            {
                return;
            }
            if(IsAttacking)
            {
                IsAttacking = false;
                if(!TransitionToAimState)
                {
                    RotateCharacterAttack(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), movement.RotateAfterAttackTime);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.aimState);
                    return;
                }

            }

            if (animationController.IsUnsheathSheathAnimPlaying)
                return;
            //Cinemachine IsBlending doesn't work properly at start
            if(stateMachine.cameraController.IsAimCameraActive) 
            {
                return;
            }
            if (stateMachine.cameraController.IsTransition)
            {
                animationController.SwordFreeMovement(Vector2.zero);
                return;
            }

            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;


            if (movementOn2DAxis.magnitude > 0.06f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
            
            if ((IsSprintHold || IsSprint) && stateMachine.stamina.UseStamina(movement.SprintStaminaCost))
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

            if (stateMachine.isRoll)
                stateMachine.ChangeState(stateMachine.rollState);
        }

        protected override void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(stateMachine.swordTargetState);
        }

        protected override void HandleSheathEvent()
        {
            if(animationController.IsAttackPlaying) return;
            stateMachine.ChangeState(stateMachine.unarmedFreeState);
            isSheating = true;
        }

        protected override void HandleOnAimHoldEvent()
        {
            if(!IsAttacking)
            {
                stateMachine.ChangeState(stateMachine.aimState);
            }
            else
            {
                TransitionToAimState= true;
            }
        }

    }
}

