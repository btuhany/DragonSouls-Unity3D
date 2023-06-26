using PlayerController;
using States;
using UnityEngine;
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
            if (stateMachine.PreviousState != stateMachine.SwordTargetState)
            {
                animationController.PlayUnsheathSword();
                animationController.FreeCombat(Weapon.Sword, false);
            }
            else
            {
                animationController.FreeCombat(Weapon.Sword, true);
            }
            base.Enter();
        }
        protected override void StateTickActions(float deltaTime)
        {
            if (animationController.IsUnsheathSheathAnimPlaying)
                return;
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
            animationController.SwordFreeMovement(movementOn2DAxis);


            if (isSprintHold || isSprint)
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.CombatSprintSpeed, deltaTime);
            else
                MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.SwordFreeSpeed, deltaTime);
            
            if (movementOn2DAxis.magnitude > 0f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
            HandleSprintControl();
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
    }
}

