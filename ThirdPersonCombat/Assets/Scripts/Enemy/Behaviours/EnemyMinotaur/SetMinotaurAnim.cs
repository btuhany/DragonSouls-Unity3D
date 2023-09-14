using UnityEngine;

public class SetMinotaurAnim : ActionNode
{
    public float transitionDuration = 0.1f;

    public bool setRun = false;
    public bool setWalk = false;
    public bool playStrafe = false;
    public bool playIdle = false;

    public bool isRun = false;
    public bool isWalk = false;

    private readonly int _animBoolRun = Animator.StringToHash("isRun");
    private readonly int _animBoolWalk = Animator.StringToHash("isWalk");
    private readonly int _animStrafeLeft = Animator.StringToHash("strafeLeft");
    private readonly int _animStrafeRight = Animator.StringToHash("strafeRight");
    private readonly int _animIdle = Animator.StringToHash("idle");
    protected override void OnStart()
    {
        if(setRun)
        {
            agent.animator.SetBool(_animBoolRun, isRun);
        }
        if(setWalk)
        {
            agent.animator.SetBool(_animBoolWalk, isWalk);
        }
        if(playStrafe)
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
        if(playIdle)
        {
            agent.animator.CrossFadeInFixedTime(_animIdle, transitionDuration);
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
