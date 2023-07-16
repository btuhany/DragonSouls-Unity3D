using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    bool _isHumanModelRotated = false;
    bool _isFastRoll = false;
    float _timeCounter;
    Vector2 _rollMovement;
    public PlayerRollState(PlayerStateMachine player) : base(player)
    {
    }

    public override void Enter()
    {
        _isFastRoll = false;
        _isHumanModelRotated = false;
        if (inputReader.MovementOn2DAxis.sqrMagnitude < 0.02f)
        {
            _rollMovement = new Vector2(stateMachine.transform.forward.x, stateMachine.transform.forward.z);  //default forward
        }
        else
        {
            _rollMovement = inputReader.MovementOn2DAxis.normalized;
        }

        

        if (stateMachine.PreviousState != stateMachine.AimState)
        {
            animationController.PlayRoll();
            RotateCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollStateRotateTime);
        }
        else
        {
            _isFastRoll = true;
            _isHumanModelRotated = true;
            movement.RotateHumanModel(Vector3.SignedAngle(stateMachine.transform.forward, new Vector3(_rollMovement.x, 0f, _rollMovement.y), Vector3.up));
            animationController.PlayFastRoll();
        }
        
        _timeCounter = 0f;
        base.Enter();
    }
    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(_isFastRoll)
        {
            if(_timeCounter > movement.FastRollDuration)
            {
                stateMachine.ChangeState(stateMachine.PreviousState);
            }
            MoveCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.FastRollDistance, deltaTime);
        }
        else
        {
            if (_timeCounter > movement.RollDuration)
            {
                //stateMachine.IsRoll = false;
                stateMachine.ChangeState(stateMachine.PreviousState);
            }
            MoveCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollDistance, deltaTime);
        }

    }
    public override void Exit()
    {
        if(_isHumanModelRotated)
        {
            movement.RotateHumanModel(0f);
        }
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
