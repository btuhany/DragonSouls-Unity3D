using UnityEngine;

public class FailureNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Child == null)
        {
            return State.Failure;
        }

        var state = Child.Update();
        if (state == State.Success)
        {
            return State.Failure;
        }
        return state;
    }
}
