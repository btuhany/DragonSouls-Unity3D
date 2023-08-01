public class SequencerNode : CompositeNode
{
    private int _currentChild;
    protected override void OnStart()
    {
        _currentChild = 0;
    }

    protected override void OnStop()
    {
       
    }

    protected override State OnUpdate()
    {
        Node child = Children[_currentChild];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                _currentChild++;
                break;
        }

        return _currentChild == Children.Count ? State.Success : State.Running;
    }
}
