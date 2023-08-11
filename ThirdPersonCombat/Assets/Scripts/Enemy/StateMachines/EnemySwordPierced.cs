using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordPierced : EnemyBaseState
{
    float _timeCounter;
    public EnemySwordPierced(EnemyStateMachine enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        _timeCounter = 0f;
        animationController.PlayGetHitAnimation(0);
    }

    public override void Exit()
    {
        stateMachine.animController.ResumeAnimation();
    }

    public override void Tick(float deltaTime)
    {
        _timeCounter += Time.deltaTime;
        if (_timeCounter >= 0.3f)
            stateMachine.animController.PauseAnimation();
    }
    public void OnGetHit()
    {
        Vector3 dir = stateMachine.health.lastDamageObj.transform.position - stateMachine.transform.position;
        dir.y = 0f;
        dir *= -1f;
        stateMachine.forceReceiver.AddForce(dir.normalized * 10f);
    }
}
