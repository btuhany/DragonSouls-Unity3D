using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNavmeshDestInRange : ActionNode
{
    public float minDistance = 0f;
    public float maxDistance = 5f;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float distance = Vector3.Distance(agent.navmeshAgent.destination, agent.transform.position);
        if (distance >= minDistance && distance < maxDistance)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
