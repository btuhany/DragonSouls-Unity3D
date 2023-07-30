using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordPierced : EnemyBaseState
{
    public EnemySwordPierced(EnemyStateMachine enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        animationController.PlayGetHitAnimation(0);
    }

    public override void Exit()
    {
       
    }

    public override void Tick(float deltaTime)
    {
        
    }
}
