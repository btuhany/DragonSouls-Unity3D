using UnityEngine;

public class RandomWaitNode : ActionNode
{
    public float minWaitTime = 0f;
    public float maxWaitTime = 2f;
    private float _waitTime;
    private float _startTime;
    protected override void OnStart()
    {
        _waitTime = Random.Range(minWaitTime, maxWaitTime);
        _startTime = Time.time;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {

        if (Time.time - _startTime >= _waitTime)
        {
            return State.Success;
        }
        return State.Running;
    }
}
