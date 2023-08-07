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
        if(stateMachine.PreviousState == stateMachine.swordTargetState || stateMachine.PreviousState == stateMachine.aimState || stateMachine.PreviousState == stateMachine.rollState)
        {
            if(!targetableCheck.TryTransferTarget())
            {
                stateMachine.ChangeState(stateMachine.unarmedFreeState);
                return;
            }
        }
        if (stateMachine.PreviousState == stateMachine.swordFreeState || stateMachine.PreviousState == stateMachine.swordTargetState)
        {
            animationController.PlaySheathSword();
            animationController.TargetCombat(Weapon.Unarmed, false);
        }
        else
        {
            animationController.TargetCombat(Weapon.Unarmed);
        }
        base.Enter();
        if (stateMachine.PreviousState == stateMachine.rollState && stateMachine.rollState.IsAttack)
        {
            LightAttack();
        }
    }
    public override void Exit()
    {
        animationController.ResetCombatBools();
        base.Exit();
    }
    protected override void StateTickActions(float deltaTime)
    {
        if (!targetableCheck.IsTargetInRange())
        {
            stateMachine.ChangeState(stateMachine.unarmedFreeState);
            return;
        }
        base.StateTickActions(deltaTime);
    }
    protected override void HandleSheathEvent()
    {
        if (animationController.IsAttackPlaying) return;
        stateMachine.ChangeState(stateMachine.swordTargetState);
    }

    protected override void HandleOnTargetEvent()
    {
        stateMachine.ChangeState(stateMachine.unarmedFreeState);
    }

    protected override void HandleOnWeaponReturn()
    {
        if (_combat.IsSwordReturned) return;
        stateMachine.ChangeState(stateMachine.returnSwordState);
    }

    protected override void HandleOnTargetSelect(Vector2 selectDir)
    {
        targetableCheck.ChangeTarget(selectDir);
    }
}
