using UnityEngine;


public class TimeoutNode : DecoratorNode
{
    [Tooltip("Returns failure after this amount of time if the subtree is still running.")] public float duration = 1.0f;
    float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
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

        if (Time.time - startTime > duration)
        {
            return State.Failure;
        }

        return Child.Update();
    }
}
