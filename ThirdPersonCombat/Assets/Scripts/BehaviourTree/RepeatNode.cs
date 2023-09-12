using UnityEngine;

public class RepeatNode : DecoratorNode
{
    [Tooltip("Restarts the subtree on success")] public bool restartOnSuccess = true;
    [Tooltip("Restarts the subtree on failure")] public bool restartOnFailure = false;
    [Tooltip("Maximum number of times the subtree will be repeated. Set to 0 to loop forever")] public int maxRepeats = 0;

    int iterationCount = 0;

    protected override void OnStart()
    {
        iterationCount = 0;
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
        switch (Child.Update())
        {
            case State.Running:
                break;
            case State.Failure:
                if (restartOnFailure)
                {
                    iterationCount++;
                    if (iterationCount == maxRepeats && maxRepeats > 0)
                    {
                        return State.Failure;
                    }
                    else
                    {
                        return State.Running;
                    }
                }
                else
                {
                    return State.Failure;
                }
            case State.Success:
                if (restartOnSuccess)
                {
                    iterationCount++;
                    if (iterationCount == maxRepeats && maxRepeats > 0)
                    {
                        return State.Success;
                    }
                    else
                    {
                        return State.Running;
                    }
                }
                else
                {
                    return State.Success;
                }
        }
        return State.Running;
    }
}


