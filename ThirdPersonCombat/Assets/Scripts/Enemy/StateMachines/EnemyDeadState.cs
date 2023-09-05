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
        if (stateMachine.isSwordOn)
        {
            stateMachine.sword.DetachFromEnemy();
            stateMachine.sword.Throwed(Vector3.down * 0.5f, PlayerStateMachine.Instance.transform);
            stateMachine.isSwordOn = false;
        }
        _timeCounter = 0f;
        animationController.PlayDeadAnimation();
        stateMachine.forceReceiver.isCharacterControllerDisabled = true;
    }

    public override void Exit()
    {

    }

    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(_timeCounter > config.DeadAnimTime)
        {
            stateMachine.isDead = true;
            stateMachine.forceReceiver.isCharacterControllerDisabled = false;
            stateMachine.gameObject.SetActive(false);
        }
    }
}
