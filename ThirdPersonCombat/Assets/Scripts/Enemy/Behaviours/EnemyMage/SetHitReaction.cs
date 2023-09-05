using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetHitReaction : ActionNode
{
    public bool hitReaction;
    protected override void OnStart()
    {
        agent.reactToHit = hitReaction;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        agent.reactToHit = hitReaction;
        return State.Success;
    }

}
