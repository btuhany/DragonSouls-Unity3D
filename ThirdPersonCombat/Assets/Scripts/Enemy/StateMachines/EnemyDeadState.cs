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
        stateMachine.forceReceiver.isCharacterControllerDisabled = true;
        if (stateMachine.isSwordOn)
        {
            stateMachine.sword.DetachFromEnemy();
            stateMachine.sword.OnEnemyDeath();
            stateMachine.isSwordOn = false;
        }
        SoulsManager.Instance.AddSoul(stateMachine.soulPoint, stateMachine.transform.position);
        _timeCounter = 0f;
        animationController.PlayDeadAnimation();
    }

    public override void Exit()
    {
        stateMachine.forceReceiver.isCharacterControllerDisabled = false;
        stateMachine.isOnDeadState = false;
    }

    public override void Tick(float deltaTime)
    {
        _timeCounter += deltaTime;
        if(_timeCounter > config.DeadAnimTime)
        {
            stateMachine.isOnDeadState = false;
            stateMachine.isDead = true;
            stateMachine.forceReceiver.isCharacterControllerDisabled = false;
            stateMachine.gameObject.SetActive(false);
        }
    }
}
