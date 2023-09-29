using States;
using UnityEngine;
public class IsPlayerInRange : ActionNode
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
        float distance = Vector3.Distance(agent.playerTransform.position, agent.transform.position);
        if(distance >= minDistance && distance < maxDistance && !PlayerStateMachine.Instance.isInvisible)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
