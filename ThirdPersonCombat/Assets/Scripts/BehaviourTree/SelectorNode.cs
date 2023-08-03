using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class SelectorNode : CompositeNode
{
    protected int current;

    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        for (int i = current; i < Children.Count; ++i)
        {
            current = i;
            var child = Children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }
        }

        return State.Failure;
    }
}
