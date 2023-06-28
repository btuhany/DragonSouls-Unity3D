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
    public override void Enter()
    {
        if (stateMachine.PreviousState == stateMachine.UnarmedTargetState)
        {
            if (!targetableCheck.TryTransferTarget())
            {
                stateMachine.ChangeState(stateMachine.SwordFreeState);
                return;
            }
        }
        if (stateMachine.PreviousState != stateMachine.SwordFreeState) //else if
        {
            animationController.PlayUnsheathSword();
            animationController.TargetCombat(Weapon.Sword,false);
        }
        else
        {
            animationController.TargetCombat(Weapon.Sword, true);
        }
        base.Enter();
    }
    public override void Exit()
    {
        animationController.ResetCombatBools();
        base.Exit();
    }
    protected override void StateTickActions(float deltaTime)
    {
        if (animationController.IsUnsheathSheathAnimPlaying)
            return;
        base.StateTickActions(deltaTime);
    }
    protected override void HandleSheathEvent()
    {
        if (animationController.IsAttackPlaying) return;
        stateMachine.ChangeState(stateMachine.UnarmedTargetState);
    }

    protected override void HandleOnTargetEvent()
    {
        stateMachine.ChangeState(stateMachine.SwordFreeState);
    }
    protected override void HandleOnAimHoldEvent()
    {
        stateMachine.ChangeState(stateMachine.AimState);
    }
}
