using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyGetHitState : EnemyBaseState
{
    float _timeCounter = 0f;
    public EnemyGetHitState(EnemyStateMachine enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        _timeCounter = 0f;
        Vector3 hitRelativePoint = stateMachine.transform.InverseTransformPoint(stateMachine.Health.LastHitPosition);
        if(hitRelativePoint.x < 0.15)
        {
            animationController.PlayGetHitAnimation(1);
        }
        else if(hitRelativePoint.x > 0.15)
        {
            animationController.PlayGetHitAnimation(-1);
        }
        else
        {
            animationController.PlayGetHitAnimation(0);
        }
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if (_timeCounter > config.GetHitAnimationTime)
            stateMachine.ChangeState(stateMachine.PreviousState);
    }
    public void GetHitAgain()
    {
        Enter();
    }
}
