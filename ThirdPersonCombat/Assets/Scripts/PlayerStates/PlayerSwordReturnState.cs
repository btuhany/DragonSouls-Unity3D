using States;
using UnityEngine;

public class PlayerSwordReturnState : PlayerBaseState
{
    public PlayerSwordReturnState(PlayerStateMachine player) : base(player)
    {
    }
    public override void Enter()
    {
        animationController.PlaySwordReturn();
        base.Enter();
    }
    public override void Tick(float deltaTime)
    {
    }
    public override void Exit()
    {
        base.Exit();
    }

    protected override void HandleOnHeavyAttackEvent()
    {
    }

    protected override void HandleOnLightAttackEvent()
    {
    }

    protected override void HandleOnTargetEvent()
    {
    }

    protected override void HandleSheathEvent()
    {
    }
}
