using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerBaseState
{
    bool _nextStateRoll = false;
    bool _aimCancelled = false;
    public bool IsFastRoll = false;
    public bool IsAttack = false;
    public bool AimHolded = false;
    public bool IsTargeted = false;
    float _timeCounter;
    Vector2 _rollMovement;
    public PlayerRollState(PlayerStateMachine player) : base(player)
    {
    }

    public override void Enter()
    {
        stateMachine.Health.IsInvulnerable = true;
        IsTargeted = false;
        AimHolded = false;
        IsAttack = false;
        _nextStateRoll = false;
        _aimCancelled = false;
        IsFastRoll = false;
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
            if(targetableCheck.TryTransferTarget())
            {
                IsTargeted = true;
            }
            else
            {
                if (stateMachine.PreviousState == stateMachine.SwordTargetState)
                    stateMachine.PreviousState = stateMachine.SwordFreeState;
                else
                    stateMachine.PreviousState = stateMachine.UnarmedFreeState;
            }
        }
        

        if (stateMachine.PreviousState != stateMachine.AimState)
        {
            animationController.PlayRoll();
            RotateCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollStateRotateTime);
        
        }
        else
        {
            if(stateMachine.AimState.IsTargeted)
            {
                if (targetableCheck.TryTransferTarget())
                {
                    IsTargeted = true;
                }
            }

            movement.RotateHumanModel(Vector3.SignedAngle(stateMachine.transform.forward, movement.CamRelativeMotionVector(_rollMovement), Vector3.up));
            IsFastRoll = true;
            animationController.PlayFastRoll();
        }
        
        _timeCounter = 0f;
        base.Enter();
    }
    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(IsFastRoll)
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

            MoveCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.FastRollDistance, deltaTime);
            
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
                if(AimHolded)
                {
                    stateMachine.AimState.IsTargeted = IsTargeted;
                    stateMachine.ChangeState(stateMachine.AimState);
                    return;
                }

                stateMachine.ChangeState(stateMachine.PreviousState);
            }

            MoveCharacter(movement.CamRelativeMotionVector(_rollMovement), movement.RollDistance, deltaTime);
            
        }

    }
    public override void Exit()
    {
        targetableCheck.ClearTarget();
        stateMachine.Health.IsInvulnerable = false;
        _nextStateRoll = false;
        if (IsFastRoll)
        {
            movement.RotateHumanModel(0f);
        }
        else
        {
            RotateCharacter(movement.CamRelativeMotionVector(inputReader.MovementOn2DAxis), movement.RollStateRotateTime);
        }
        stateMachine.IsRoll = false;
        base.Exit();
    }
    protected override void HandleOnHeavyAttackEvent()
    {
        IsAttack = true;
    }

    protected override void HandleOnLightAttackEvent()
    {
        IsAttack = true;
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

    protected override void HandleOnAimHoldEvent()
    {
        AimHolded = true;
    }

    protected override void HandleOnRollEvent()
    {
        _nextStateRoll = true;
    }
}
