using PlayerController;
using States;
using UnityEngine;

namespace States
{
    public class PlayerUnarmedFreeState : PlayerCombatFreeState
    {
        public PlayerUnarmedFreeState(PlayerStateMachine player, Weapon weapon = Weapon.Unarmed, bool entryAttack = true, bool autoStateChange = false) : base(player, weapon, entryAttack, autoStateChange)
        {
        }

        protected override void StateTickActions(float deltaTime)
        {
            Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
            animationController.UnarmedFreeMovement(movementOn2DAxis);
            MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.FreeLookMaxMovementSpeed, deltaTime);
            if (movementOn2DAxis.magnitude > 0f)
            {
                RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
            }
        }
        protected override void HandleOnTargetEvent()
        {
            if (!targetableCheck.TrySelectTarget()) return;
            stateMachine.ChangeState(stateMachine.UnarmedTargetState);
        }
    }
}
