using PlayerController;
using UnityEngine;
namespace States
{
    public class PlayerCombatFreeState : PlayerCombatState
    {
        public PlayerCombatFreeState(PlayerStateMachine player, Weapon weapon, bool autoStateChange = false) : base(player, weapon, autoStateChange)
        {
        }

        protected override void StateTickActions(float deltaTime)
        {
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
            animationController.UnarmedModeMovement(movementOn2DAxis);
            MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.FreeLookMaxMovementSpeed, deltaTime);
            if (movementOn2DAxis.magnitude > 0f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
        }

        protected override void StateEnterActions()
        {
            animationController.SetBoolsCombatFree();
            if(inputReader.LastAttackType == AttackType.Light)
            {
                LightAttack();
            }
            else if(inputReader.LastAttackType == AttackType.Heavy)
            {
                HeavyAttack();
            }
        }

        protected override void StateExitActions()
        {
            animationController.CancelTargetBools();
        }
    }
}

