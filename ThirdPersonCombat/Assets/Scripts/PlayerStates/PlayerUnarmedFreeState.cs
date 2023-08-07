using PlayerController;
using States;
using UnityEngine;
using Combat;
namespace States
{
    public class PlayerUnarmedFreeState : PlayerCombatFreeState
    {
        private bool _isSheath = false;
        
        public PlayerUnarmedFreeState(PlayerStateMachine player, Weapon weapon = Weapon.Unarmed, bool entryAttack = true, bool autoStateChange = false) : base(player, weapon, entryAttack, autoStateChange)
        {
        }
        public override void Enter()
        {
            if(stateMachine.PreviousState == stateMachine.swordFreeState || stateMachine.PreviousState == stateMachine.swordTargetState)
            {
                animationController.PlaySheathSword();
                animationController.FreeCombat(weapon, false);
                bool currentEntryAttack = entryAttack;
                entryAttack = false;
                base.Enter();
                entryAttack = currentEntryAttack;
                _isSheath = true;
            }
            else if(stateMachine.PreviousState == stateMachine.aimState)
            {
                _isSheath = false;
                animationController.FreeCombat(weapon, false);
                bool currentEntryAttack = entryAttack;
                entryAttack = false;
                base.Enter();
                entryAttack = currentEntryAttack;
            }
            else if(stateMachine.PreviousState == stateMachine.rollState)
            {
                _isSheath = false;
                if (stateMachine.rollState.IsAttack)
                {
                    entryAttack = true;
                }
                else
                {
                    entryAttack = false;
                }
                animationController.FreeCombat(weapon);
                base.Enter();
            }
            else
            {
                _isSheath = false;
                animationController.FreeCombat(weapon);
                base.Enter();
            }
        }

        protected override void StateTickActions(float deltaTime)
        {
            if (IsAttacking)
            {
                IsAttacking = false;
                RotateCharacterAttack(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), movement.RotateAfterAttackTime);
            }
            if (_isSheath)
            {
                if (animationController.IsUnsheathSheathAnimPlaying)
                {
                    return;
                }
            }
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;

            if ((IsSprintHold || IsSprint) && stateMachine.stamina.UseStamina(movement.SprintStaminaCost))
            {
                animationController.SprintSetFloats(movementOn2DAxis);
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.CombatSprintSpeed, deltaTime);
            }
            else
            {
                animationController.UnarmedFreeMovement(movementOn2DAxis);
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.UnarmedFreeSpeed, deltaTime);
            }

            if (movementOn2DAxis.magnitude > 0f)
            {
                if (stateMachine.cameraController.IsTransition) 
                {
                    return;
                }
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
            HandleSprintControl();

            if (stateMachine.isRoll)
                stateMachine.ChangeState(stateMachine.rollState);
        }
        protected override void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(stateMachine.unarmedTargetState);
        }

        protected override void HandleSheathEvent()
        {
            if (animationController.IsAttackPlaying || !_combat.IsSwordReturned) return;
            stateMachine.ChangeState(stateMachine.swordFreeState);
        }
        protected override void HandleOnWeaponReturn()
        {
            if (_combat.IsSwordReturned) return;
            stateMachine.ChangeState(stateMachine.returnSwordState);
        }
    }
}
