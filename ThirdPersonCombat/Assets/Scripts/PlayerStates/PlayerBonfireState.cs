using States;
using UnityEngine;

public class PlayerBonfireState : PlayerBaseState
{
    private const float _cancelButtonActiveWait = 3f;
    private float _timeCounter = 0f;
    private bool _isWaitTimePassed = false;
    public PlayerBonfireState(PlayerStateMachine player) : base(player)
    {
    }
    public override void Enter()
    {
        stateMachine.ResetSetHealFlask();
        _isWaitTimePassed = false;
        _timeCounter = 0f;
        animationController.PlayBonfireSit();
        inputReader.RollEvent += HandleOnRollEvent;
    }
    public override void Exit()
    {
        BonfiresManager.Instance.LastInteractedBonfire.AtBonfire();
        inputReader.RollEvent -= HandleOnRollEvent;
    }
    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(_timeCounter >= _cancelButtonActiveWait)
        {
            _timeCounter = 0f;
            _isWaitTimePassed = true;
        }
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
        if (!_isWaitTimePassed) return;
        BonfiresManager.Instance.ExitRest();
        stateMachine.ChangeState(stateMachine.PreviousState);
    }
}
