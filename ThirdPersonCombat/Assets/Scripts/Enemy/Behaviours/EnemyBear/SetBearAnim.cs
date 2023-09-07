using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBearAnim : ActionNode
{
    public float transitionDuration = 0.1f;
    [Header("Anims To Set")]
    public bool setRunForward = false;
    public bool setRunBackward= false;
    [Header("Anims To Set")]
    public bool isRunForward = false;
    public bool isRunBackward = false;

    private readonly int _animRunForward = Animator.StringToHash("runForward");
    private readonly int _animRunBackward = Animator.StringToHash("runBackward");
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (setRunForward)
        {
            agent.animator.SetBool(_animRunForward, isRunForward);
        }
        else if (setRunBackward)
        {
            agent.animator.SetBool(_animRunBackward, isRunBackward);
        }
        return State.Success;
    }
}
