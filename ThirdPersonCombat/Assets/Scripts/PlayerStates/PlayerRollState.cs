using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    float _timeCounter;
    Vector2 _rollMovement;
    public PlayerRollState(PlayerStateMachine player) : base(player)
    {
    }

    public override void Enter()
    {
        if (inputReader.MovementOn2DAxis.sqrMagnitude < 0.02f)
        {
            _rollMovement = Vector2.up.normalized;  //default
        }
        else
        {
            _rollMovement = inputReader.MovementOn2DAxis.normalized;
        }
        RotateCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollStateRotateTime);
        animationController.PlayRoll();
        _timeCounter = 0f;
        base.Enter();
    }
    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(_timeCounter > movement.RollDuration)
        {
            //stateMachine.IsRoll = false;
            stateMachine.ChangeState(stateMachine.PreviousState);
        }
        MoveCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollDistance, deltaTime);
    }
    public override void Exit()
    {
        stateMachine.IsRoll = false;
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
