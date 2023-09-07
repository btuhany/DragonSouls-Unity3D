using UnityEngine;

public class ProbabilityNode : DecoratorNode
{
    public float probability;
    private bool _isSuccessed = false;
    protected override void OnStart()
    {
        int randomNum = Random.Range(0, 100);
        if (100 - 100 * probability <= randomNum)
        {
            _isSuccessed = true;
        }
        else
        {
            _isSuccessed = false;
        }
        //Debug.Log("state: " + _isSuccessed.ToString());// + " with random num:" + randomNum.ToString()); 
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
