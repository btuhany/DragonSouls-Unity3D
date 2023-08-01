public class RepeatNode : DecoratorNode
{
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        Child.Update();
        return State.Running;
    }
}
