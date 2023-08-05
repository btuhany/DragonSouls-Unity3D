using States;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayer : ActionNode
{

    public float chaseRotationLerpTime = 1.5f;
    public float speed = 1.6f;
    public float stopDistance = 2f;
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
        _navmeshAgent.destination = PlayerStateMachine.Instance.transform.position;
        if (!agent.forceReceiver.IsGrounded) return State.Running;
        agent.characterController.Move(_navmeshAgent.desiredVelocity.normalized * speed * Time.deltaTime);
        _navmeshAgent.velocity = agent.characterController.velocity; 

        //FaceToPlayer
        Vector3 dir = agent.characterController.velocity.normalized;
        dir.y = 0f;
        agent.locomotion.LookRotation(dir, chaseRotationLerpTime, Time.deltaTime);

        return State.Success;

    }
}
