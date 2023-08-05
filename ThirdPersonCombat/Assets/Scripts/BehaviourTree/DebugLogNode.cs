using UnityEngine;

public class DebugLogNode : ActionNode
{
    public string Message;
    protected override void OnStart()
    {
        Debug.Log($"OnStart{Message}");
    }

    protected override void OnStop()
    {
        Debug.Log($"OnStop{Message}");
    }

    protected override State OnUpdate()
    {
        Debug.Log($"OnUpdate{Message}");
        return State.Success;
    }
}
