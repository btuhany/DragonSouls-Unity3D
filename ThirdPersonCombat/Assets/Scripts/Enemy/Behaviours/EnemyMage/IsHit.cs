public class IsHit : ActionNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if(blackboard.isHit)
        {
            blackboard.isHit = false;
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
        
    }
}
