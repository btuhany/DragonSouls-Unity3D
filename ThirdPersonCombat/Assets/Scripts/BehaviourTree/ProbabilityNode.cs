using UnityEngine;

public class ProbabilityNode : DecoratorNode
{
    public float probability;
    private bool _isSuccessed = false;
    protected override void OnStart()
    {
        if (100 - 100 * probability <= Random.Range(0, 100))
        {
            _isSuccessed = true;
        }
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (_isSuccessed)
        {
            return Child.Update();
        }

        return State.Failure;
    }
}
