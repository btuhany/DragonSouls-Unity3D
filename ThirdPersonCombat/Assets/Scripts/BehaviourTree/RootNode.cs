using UnityEngine;

public class RootNode : Node
{
    public Node Child;
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        return Child.Update();
    }

    public override Node Clone()
    {
        RootNode node = Instantiate(this);
        node.Child = Child.Clone();
        return node;
    }
}
