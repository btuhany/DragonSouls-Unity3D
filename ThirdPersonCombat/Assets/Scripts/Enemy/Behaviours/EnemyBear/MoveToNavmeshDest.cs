using UnityEngine;
using UnityEngine.AI;

public class MoveToNavmeshDest : ActionNode
{
    public bool enableRotation = false;
    public float speed = 5f;
    public float rotationSpeed = 3f;
    private NavMeshAgent _navmeshAgent;
    public bool variableSpeedForDist;
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
        if(variableSpeedForDist)
        {
            float speedFactor = 0f;
            float distance = Vector3.Distance(_navmeshAgent.destination, agent.transform.position);
            if(distance < 5)
            {
                speedFactor = 1.5f;
            }
            else if(distance < 10)
            {
                speedFactor = 2f;
            }
            else if(distance < 17)
            {
                speedFactor = 3f;
            }
            else if(distance < 27)
            {
                speedFactor = 4.7f;
            }
            else if(distance < 45)
            {
                speedFactor = 6f;
            }
            else if(distance < 75)
            {
                speedFactor = 9f;
            }
            agent.characterController.Move(_navmeshAgent.desiredVelocity.normalized * speed * Time.deltaTime * speedFactor);
        }
        else
        {
            agent.characterController.Move(_navmeshAgent.desiredVelocity.normalized * speed * Time.deltaTime);
        }
        _navmeshAgent.velocity = agent.characterController.velocity;

        if(enableRotation)
        {
            Vector3 dir = agent.characterController.velocity.normalized;
            dir.y = 0f;
            agent.locomotion.LookRotation(dir, rotationSpeed, Time.deltaTime);
        }

        return State.Success;
    }
}
