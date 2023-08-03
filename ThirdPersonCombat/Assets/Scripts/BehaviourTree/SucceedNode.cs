using UnityEngine;

public class SucceedNode : DecoratorNode
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
        if (state == State.Failure)
        {
            return State.Success;
        }
        return state;
    }
}
