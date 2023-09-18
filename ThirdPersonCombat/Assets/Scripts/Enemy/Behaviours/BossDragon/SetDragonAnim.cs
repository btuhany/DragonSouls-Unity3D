using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SetDragonAnim : ActionNode
{
    public float transitionDuration = 0.1f;

    public bool playIdle1 = false;
    public bool playIdle2 = false;
    public bool playSleep = false;
    public bool playStrafe = false;

    public bool setRun = false;
    public bool setWalk = false;

    public bool isRun = false;
    public bool isWalk = false;

    private readonly int _animBoolRun = Animator.StringToHash("isRun");
    private readonly int _animBoolWalk = Animator.StringToHash("isWalk");
    private readonly int _animStrafeLeft = Animator.StringToHash("strafeLeft");
    private readonly int _animStrafeRight = Animator.StringToHash("strafeRight");
    private readonly int _animIdle = Animator.StringToHash("idle");
    private readonly int _animIdle2 = Animator.StringToHash("idle2");
    private readonly int _animSleep = Animator.StringToHash("Sleep");
    protected override void OnStart()
    {
        if (setRun)
        {
            agent.animator.SetBool(_animBoolRun, isRun);
        }
        if (setWalk)
        {
            agent.animator.SetBool(_animBoolWalk, isWalk);
        }
        if (playStrafe)
        {
            Vector3 targetRelativePoint = agent.transform.InverseTransformPoint(agent.playerTransform.position);
            if (targetRelativePoint.x <= 0)
            {
                agent.animator.CrossFadeInFixedTime(_animStrafeLeft, transitionDuration);
                blackboard.playerOnLeft = true;
            }
            else
            {
                agent.animator.CrossFadeInFixedTime(_animStrafeRight, transitionDuration);
                blackboard.playerOnLeft = false;
            }
        }
        if (playIdle1)
        {
            agent.animator.CrossFadeInFixedTime(_animIdle, transitionDuration);
        }
        if (playIdle2)
        {
            agent.animator.CrossFadeInFixedTime(_animIdle2, transitionDuration);
        }
        if (playSleep)
        {
            agent.animator.CrossFadeInFixedTime(_animSleep, transitionDuration);
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
