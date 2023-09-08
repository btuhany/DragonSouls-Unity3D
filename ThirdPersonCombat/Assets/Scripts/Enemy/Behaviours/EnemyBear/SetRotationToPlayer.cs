using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotationToPlayer : ActionNode
{
    public bool faceToPlayer = false;
    public float faceRotationLerpTime = 2f;
    protected override void OnStart()
    {
        if (agent.isFaceToPlayer) { return; }
        agent.faceToPlayer = faceToPlayer;
        //Debug.Log($"agent.FaceToPlayer: {agent.faceToPlayer}");
        //Debug.Log($"faceToPlayer: {faceToPlayer}");
        if (!faceToPlayer)
        {
            return;
        }
        agent.faceLerpTime = faceRotationLerpTime;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }

}
