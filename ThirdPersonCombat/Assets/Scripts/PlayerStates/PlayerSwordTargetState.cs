using PlayerController;
using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordTargetState : PlayerCombatTargetState
{
    public PlayerSwordTargetState(PlayerStateMachine player, Weapon weapon = Weapon.Sword, bool autoStateChange = false) : base(player, weapon, autoStateChange)
    {
    }

    protected override void StateEnterActions()
    {
        base.StateEnterActions();
        animationController.PlaySetBoolsCombatTarget(Weapon.Sword);
    }
}
