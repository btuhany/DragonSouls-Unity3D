using PlayerController;
using States;
using UnityEngine;

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
            if(stateMachine.PreviousState == stateMachine.SwordFreeState || stateMachine.PreviousState == stateMachine.SwordTargetState)
            {
                animationController.PlaySheathSword();
                animationController.FreeCombat(weapon, false);
                bool currentEntryAttack = entryAttack;
                entryAttack = false;
                base.Enter();
                entryAttack = currentEntryAttack;
                _isSheath = true;
            }
            else if(stateMachine.PreviousState == stateMachine.AimState)
            {
                _isSheath = false;
                bool currentEntryAttack = entryAttack;
                entryAttack = false;
                base.Enter();
                entryAttack = currentEntryAttack;
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
            if(_isSheath)
            {
                if (animationController.IsUnsheathSheathAnimPlaying)
                    return;
            }
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
            animationController.UnarmedFreeMovement(movementOn2DAxis);

            if (isSprintHold || isSprint)
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.CombatSprintSpeed, deltaTime);
            else
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.UnarmedFreeSpeed, deltaTime);

            if (movementOn2DAxis.magnitude > 0f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
            HandleSprintControl();
        }

        protected override void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(stateMachine.UnarmedTargetState);
        }

        protected override void HandleSheathEvent()
        {
            if (animationController.IsAttackPlaying) return;
            stateMachine.ChangeState(stateMachine.SwordFreeState);
        }
        protected override void HandleOnWeaponReturn()
        {
            animationController.PlaySwordReturn();
            _combat.TryReturnSword();
        }
    }
}
