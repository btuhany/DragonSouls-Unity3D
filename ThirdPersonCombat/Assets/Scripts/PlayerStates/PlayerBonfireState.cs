using States;
using UnityEngine;

public class PlayerBonfireState : PlayerBaseState
{
    public PlayerBonfireState(PlayerStateMachine player) : base(player)
    {
    }
    public override void Enter()
    {
        animationController.PlayBonfireSit();
        inputReader.RollEvent += HandleOnRollEvent;
    }
    public override void Exit()
    {
        BonfiresManager.Instance.lastInteractedBonfire.AtBonfire();
        inputReader.RollEvent -= HandleOnRollEvent;
    }
    public override void Tick(float deltaTime)
    {
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
    protected override void HandleOnRollEvent()
    {
        stateMachine.ChangeState(stateMachine.PreviousState);
    }
}
