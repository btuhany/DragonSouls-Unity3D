using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsAttackEnded : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        blackboard.attackTimeCounter += Time.deltaTime;
        if(blackboard.attackTimeCounter >= agent.combat.CurrentAttack.attackDuration)
        {
            blackboard.attackTimeCounter = 0;
            return State.Success;
        }
        return State.Running;
    }
}
