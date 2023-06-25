using PlayerController;
using States;
using UnityEngine;

public class PlayerUnarmedFreeState : PlayerCombatFreeState
{
    public PlayerUnarmedFreeState(PlayerStateMachine player, Weapon weapon = Weapon.Unarmed, bool autoStateChange = false) : base(player, weapon, autoStateChange)
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
}
