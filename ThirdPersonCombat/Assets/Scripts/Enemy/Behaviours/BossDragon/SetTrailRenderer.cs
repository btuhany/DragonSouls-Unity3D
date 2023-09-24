using UnityEngine;

public class SetTrailRenderer : ActionNode
{
    public bool isActive;
    protected override void OnStart()
    {
        agent.SetTrailRenderer(isActive);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
