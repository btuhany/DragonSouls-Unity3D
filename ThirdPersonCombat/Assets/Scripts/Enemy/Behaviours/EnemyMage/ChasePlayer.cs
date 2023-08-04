using States;
using UnityEngine;
using UnityEngine.AI;

public class ChasePlayer : ActionNode
{
    private readonly int _animWalk = Animator.StringToHash("Walk");
    public float chaseRotationLerpTime = 1.5f;
    public float speed = 1.6f;
    public float stopDistance = 2f;
    private NavMeshAgent _navmeshAgent;
    protected override void OnStart()
    {
        _navmeshAgent = agent.navmeshAgent;
        _navmeshAgent.isStopped = false;
        _navmeshAgent.updatePosition = false;
        _navmeshAgent.destination = PlayerStateMachine.Instance.transform.position;
        agent.animator.CrossFadeInFixedTime(_animWalk, 0.1f);
    }

    protected override void OnStop()
    {
        _navmeshAgent.isStopped = true;
        _navmeshAgent.updatePosition = true;
        _navmeshAgent.ResetPath();
        _navmeshAgent.velocity = Vector3.zero;
        agent.animator.speed = 1f;
    }

    protected override State OnUpdate()
    {
         _navmeshAgent.destination = PlayerStateMachine.Instance.transform.position;
        if (!agent.forceReceiver.IsGrounded) return State.Running;
        agent.characterController.Move(_navmeshAgent.desiredVelocity.normalized * speed * Time.deltaTime);
        _navmeshAgent.velocity = agent.characterController.velocity;

        Vector3 dir = agent.characterController.velocity.normalized;
        dir.y = 0f;
        agent.locomotion.LookRotation(dir, chaseRotationLerpTime, Time.deltaTime);

        if (Vector3.Distance(agent.transform.position, agent.playerTransform.position) <= stopDistance)
            return State.Success;
        else
            return State.Running;



    }
}
