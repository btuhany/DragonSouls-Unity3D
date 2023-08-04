using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    float _timeCounter = 0.0f;
    public EnemyDeadState(EnemyStateMachine enemy) : base(enemy)
    {
    }

    public override void Enter() 
    {
        if (stateMachine.IsSwordOn)
        {
            stateMachine.Sword.DetachFromEnemy();
            stateMachine.Sword.Throwed(Vector3.down * 0.5f, PlayerStateMachine.Instance.transform);
            stateMachine.IsSwordOn = false;
        }
        _timeCounter = 0f;
        animationController.PlayDeadAnimation();
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(_timeCounter > config.DeadAnimTime)
        {
            stateMachine.gameObject.SetActive(false);
            stateMachine.IsDead = true;
        }
    }
}
