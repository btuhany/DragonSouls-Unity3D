using UnityEngine;
using States;
using UnityEngine.AI;

public class SetNavmesh : ActionNode
{

    public bool enabled = false;
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
        if(enabled && _navmeshAgent.isStopped)
        {
            _navmeshAgent.isStopped = false;
            _navmeshAgent.updatePosition = false;
            _navmeshAgent.updateRotation = false;
            _navmeshAgent.ResetPath();
            _navmeshAgent.velocity = Vector3.zero;

            _navmeshAgent.destination = PlayerStateMachine.Instance.transform.position;
        }
        else if(!enabled && !_navmeshAgent.isStopped)
        {
            _navmeshAgent.isStopped = true;
            _navmeshAgent.updateRotation = true;
            _navmeshAgent.updatePosition = true;
            _navmeshAgent.ResetPath();
            _navmeshAgent.velocity = Vector3.zero;

        }
        return State.Success;
    }
}
