using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAudioSource : ActionNode
{
    public bool isPlay = false;
    protected override void OnStart()
    {
        agent.SetSecondaryAuidoSource(isPlay);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
