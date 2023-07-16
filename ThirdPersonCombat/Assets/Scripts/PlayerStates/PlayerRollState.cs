using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    bool _isTargeted;
    bool _nextStateRoll = false;
    bool _aimCancelled = false;
    bool _isFastRoll = false;
    float _timeCounter;
    Vector2 _rollMovement;
    public PlayerRollState(PlayerStateMachine player) : base(player)
    {
    }

    public override void Enter()
    {
        _nextStateRoll = false;
        _aimCancelled = false;
        _isFastRoll = false;
        if (inputReader.MovementOn2DAxis.sqrMagnitude < 0.02f)
        {
            _rollMovement = new Vector2(stateMachine.transform.forward.x, stateMachine.transform.forward.z);  //default forward
        }
        else
        {
            _rollMovement = inputReader.MovementOn2DAxis.normalized;
        }
        
        if(stateMachine.PreviousState == stateMachine.SwordTargetState || stateMachine.PreviousState == stateMachine.UnarmedTargetState)
        {
            _isTargeted = true;
            if(!targetableCheck.TryTransferTarget())
            {
                Debug.Log("TODO!");
            }
        }
        

        if (stateMachine.PreviousState != stateMachine.AimState)
        {
            animationController.PlayRoll();
            if(_isTargeted)
            {
                RotateCharacter(movement.TargetRelativeMotionVector(_rollMovement), movement.RollStateRotateTime);
            }
            else
            {
                RotateCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollStateRotateTime);
            }
        }
        else
        {
            _isFastRoll = true;
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
                if (_nextStateRoll)
                {
                    Exit();
                    Enter();
                    return;
                }
                if (_aimCancelled)
                {
                    if (stateMachine.AimState.IsTargeted)
                    {
                        stateMachine.ChangeState(stateMachine.SwordTargetState);
                    }
                    else
                    {
                        stateMachine.ChangeState(stateMachine.SwordFreeState);
                    }
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.PreviousState);
                }
            }
            if(_isTargeted)
            {
                MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
            }
            else
            {
                MoveCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.FastRollDistance, deltaTime);
            }
        }
        else
        {
            if (_timeCounter > movement.RollDuration)
            {
                if (_nextStateRoll)
                {
                    Exit();
                    Enter();
                    return;
                }
                //stateMachine.IsRoll = false;
                stateMachine.ChangeState(stateMachine.PreviousState);
            }
            if(_isTargeted)
            {
                MoveCharacter(MotionVectorAroundTarget(), movement.TargetMovementSpeed, deltaTime);
            }
            else
            {
                MoveCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollDistance, deltaTime);
            }
        }

    }
    public override void Exit()
    {
        _nextStateRoll = false;
        if (_isFastRoll)
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

    protected override void HandleOnAimHoldCancelEvent()
    {
        _aimCancelled = true;
    }

    protected override void HandleOnRollEvent()
    {
        _nextStateRoll = true;
    }

    private Vector3 MotionVectorAroundTarget()
    {
        //Character always looks to target
        Vector3 motion = Vector3.zero;
        motion += transform.right * inputReader.MovementOn2DAxis.x;
        motion += transform.forward * inputReader.MovementOn2DAxis.y;
        return motion;
    }
}
