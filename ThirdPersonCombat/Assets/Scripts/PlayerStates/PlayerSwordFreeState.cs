using PlayerController;
using States;
using UnityEngine;
public class PlayerSwordFreeState : PlayerCombatFreeState
{
    public PlayerSwordFreeState(PlayerStateMachine player, Weapon weapon = Weapon.Sword, bool entryAttack = false, bool autoStateChange = false) : base(player, weapon, entryAttack, autoStateChange)
    {
    }

    protected override void StateTickActions(float deltaTime)
    {
        if (animationController.IsUnsheathAnimInfo)
            return;
        Vector2 movementOn2DAxis = inputReader.MovementOn2DAxis;
        animationController.SwordFreeMovement(movementOn2DAxis);
        MoveCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), movement.FreeLookMaxMovementSpeed, deltaTime);
        if (movementOn2DAxis.magnitude > 0f)
        {
            RotateCharacter(movement.CamRelativeMotionVector(movementOn2DAxis), deltaTime);
        }
    }
}
