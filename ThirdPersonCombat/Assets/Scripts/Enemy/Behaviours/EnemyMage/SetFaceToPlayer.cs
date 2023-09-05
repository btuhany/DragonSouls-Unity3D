using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFaceToPlayer : ActionNode
{
    public bool faceToPlayer = false;
    public float faceRotationLerpTime = 2f;
    public float animCrossFade = 0.1f;

    private readonly int _animLeftStare = Animator.StringToHash("leftstare");
    private readonly int _animRightStare = Animator.StringToHash("rightstare");
    protected override void OnStart()
    {
        if(agent.isFaceToPlayer) { return; }
        agent.faceToPlayer = faceToPlayer;
        //Debug.Log($"agent.FaceToPlayer: {agent.faceToPlayer}");
        //Debug.Log($"faceToPlayer: {faceToPlayer}");
        if (!faceToPlayer)
        {
            return;
        }
        agent.faceLerpTime = faceRotationLerpTime;

        Vector3 targetRelativePoint = agent.transform.InverseTransformPoint(agent.playerTransform.position);
        if (targetRelativePoint.x <= 0)
        {
            agent.animator.CrossFadeInFixedTime(_animLeftStare, animCrossFade);
        }
        else
        {
            agent.animator.CrossFadeInFixedTime(_animRightStare, animCrossFade);
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }

}
