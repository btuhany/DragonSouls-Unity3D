using States;
using UnityEngine;
public class PlayerDeadState : PlayerBaseState
{
    private float _deathWaitTime = 5.5f;
    private float _timeCounter = 0f;
    public PlayerDeadState(PlayerStateMachine player) : base(player)
    {
    }
    public override void Enter()
    {
        stateMachine.health.IsInvulnerable = true;
        _timeCounter = 0f;
        animationController.PlayDeath();
        if (targetableCheck.IsThereTarget)
            targetableCheck.ClearTarget();
    }
    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(_timeCounter > _deathWaitTime)
        {
            _timeCounter = 0f;
            stateMachine.Respawn();
        }
    }



    public override void Exit()
    {
        stateMachine.health.IsInvulnerable = false;
        _timeCounter = 0f;
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
