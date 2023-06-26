using PlayerController;
using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnarmedTargetState : PlayerCombatTargetState
{
    public PlayerUnarmedTargetState(PlayerStateMachine player, Weapon weapon = Weapon.Unarmed, bool autoStateChange = false) : base(player, weapon, autoStateChange)
    {
    }
    //protected override void StateTickActions(float deltaTime)
    //{
    //    base.StateTickActions(deltaTime);
    //}
    protected override void StateEnterActions()
    {
        animationController.PlaySetBoolsCombatTarget(Weapon.Unarmed);
        base.StateEnterActions();
    }
}
