using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{
    public EnemyDeadState(EnemyStateMachine enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        animationController.PlayDeadAnimation();
    }

    public override void Exit()
    {
    }

    public override void Tick(float deltaTime)
    {
    }
}
