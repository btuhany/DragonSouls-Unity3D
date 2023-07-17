using PlayerController;
using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat;
public class PlayerUnarmedTargetState : PlayerCombatTargetState
{
    public PlayerUnarmedTargetState(PlayerStateMachine player, Weapon weapon = Weapon.Unarmed, bool autoStateChange = false) : base(player, weapon, autoStateChange)
    {
    }
    public override void Enter()
    {
        if(stateMachine.PreviousState == stateMachine.SwordTargetState || stateMachine.PreviousState == stateMachine.AimState || stateMachine.PreviousState == stateMachine.RollState)
        {
            if(!targetableCheck.TryTransferTarget())
            {
                stateMachine.ChangeState(stateMachine.UnarmedFreeState);
                return;
            }
        }
        if (stateMachine.PreviousState == stateMachine.SwordFreeState || stateMachine.PreviousState == stateMachine.SwordTargetState)
        {
            animationController.PlaySheathSword();
            animationController.TargetCombat(Weapon.Unarmed, false);
        }
        else
        {
            animationController.TargetCombat(Weapon.Unarmed);
        }
        base.Enter();
    }
    public override void Exit()
    {
        animationController.ResetCombatBools();
        base.Exit();
    }
    protected override void HandleSheathEvent()
    {
        if (animationController.IsAttackPlaying) return;
        stateMachine.ChangeState(stateMachine.SwordTargetState);
    }

    protected override void HandleOnTargetEvent()
    {
        stateMachine.ChangeState(stateMachine.UnarmedFreeState);
    }

    protected override void HandleOnWeaponReturn()
    {
        if (_combat.IsSwordReturned) return;
        stateMachine.ChangeState(stateMachine.ReturnSwordState);
    }

    protected override void HandleOnTargetSelect(Vector2 selectDir)
    {
        targetableCheck.ChangeTarget(selectDir);
    }
}
