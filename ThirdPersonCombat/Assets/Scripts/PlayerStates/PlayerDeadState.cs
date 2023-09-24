using States;


public class PlayerDeadState : PlayerBaseState
{
    public PlayerDeadState(PlayerStateMachine player) : base(player)
    {
    }
    public override void Enter()
    {
        animationController.PlayDeath();
        if (targetableCheck.IsThereTarget)
            targetableCheck.ClearTarget();
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
}
