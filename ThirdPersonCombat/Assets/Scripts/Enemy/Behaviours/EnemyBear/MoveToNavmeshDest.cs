using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToNavmeshDest : ActionNode
{
    public bool enableRotation = false;
    private NavMeshAgent _navmeshAgent;
    protected override void OnStart()
    {
        _navmeshAgent = agent.navmeshAgent;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (!agent.forceReceiver.isGrounded) return State.Running;
        agent.characterController.Move(_navmeshAgent.desiredVelocity.normalized * 7f * Time.deltaTime);
        _navmeshAgent.velocity = agent.characterController.velocity;

        if(enableRotation)
        {
            Vector3 dir = agent.characterController.velocity.normalized;
            dir.y = 0f;
            agent.locomotion.LookRotation(dir, 1.5f, Time.deltaTime);
        }

        return State.Success;
    }
}
