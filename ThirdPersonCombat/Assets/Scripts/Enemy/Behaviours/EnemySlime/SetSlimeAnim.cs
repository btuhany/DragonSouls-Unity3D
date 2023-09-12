using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSlimeAnim : ActionNode
{
    public float transitionDuration = 0.1f;

    public bool isRunForward = false;

    private readonly int _animRun = Animator.StringToHash("run");

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        agent.animator.SetBool(_animRun, isRunForward);
        return State.Success;
    }

    protected override void OnStart()
    {
    }
}
