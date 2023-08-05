using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterruptSelector : SelectorNode
{
    protected override State OnUpdate()
    {
        int previous = current;
        base.OnStart();
        var status = base.OnUpdate();
        if (previous != current)
        {
            if (Children[previous].mState == State.Running)
            {
                Children[previous].Abort();
            }
        }

        return status;
    }
}
