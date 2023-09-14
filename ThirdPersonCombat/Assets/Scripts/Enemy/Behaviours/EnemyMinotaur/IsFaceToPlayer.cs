using States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFaceToPlayer : ActionNode
{
    public float minSimilarity = 0.99f;


    private readonly int _animStrafeLeft = Animator.StringToHash("strafeLeft");
    private readonly int _animStrafeRight = Animator.StringToHash("strafeRight");
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 dir = PlayerStateMachine.Instance.transform.position - agent.transform.position;
        dir.y = 0f;
        float similarity = Vector3.Dot(dir.normalized, agent.transform.forward);

        if (similarity > minSimilarity)
            return State.Success;

        Vector3 targetRelativePoint = agent.transform.InverseTransformPoint(agent.playerTransform.position);
        if(blackboard.playerOnLeft)
        {
            if (targetRelativePoint.x > 0)
            {
                agent.animator.CrossFadeInFixedTime(_animStrafeRight, 0.1f);
                blackboard.playerOnLeft = false;
            }
        }
        else
        {
            if (targetRelativePoint.x <= 0)
            {
                agent.animator.CrossFadeInFixedTime(_animStrafeLeft, 0.1f);
                blackboard.playerOnLeft = true;
            }
        }

        return State.Failure;
    }
}
