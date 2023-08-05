using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAnim : ActionNode
{
    public float transitionDuration = 0.1f;
    [Header("Anims To Set")]
    public bool setWalk = false;
    public bool setJump = false;
    [Header("Anims To Set")]
    public bool isWalk = false;
    public bool isJump = false;

    private readonly int _animIsWalk = Animator.StringToHash("IsWalk");
    private readonly int _animIsJump = Animator.StringToHash("IsJump");
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (setWalk)
        {
            agent.animator.SetBool(_animIsWalk, isWalk);
        }
        else if (setJump)
        {
            agent.animator.SetBool(_animIsJump, isJump);
        }
        return State.Success;
    }
}
