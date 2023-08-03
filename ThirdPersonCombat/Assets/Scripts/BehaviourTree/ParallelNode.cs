using System.Collections.Generic;
using System.Linq;

public class ParallelNode : CompositeNode
{
    List<State> childrenLeftToExecute = new List<State>();

    protected override void OnStart()
    {
        childrenLeftToExecute.Clear();
        Children.ForEach(a => {
            childrenLeftToExecute.Add(State.Running);
        });
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        bool stillRunning = false;
        for (int i = 0; i < childrenLeftToExecute.Count(); ++i)
        {
            if (childrenLeftToExecute[i] == State.Running)
            {
                var status = Children[i].Update();
                if (status == State.Failure)
                {
                    AbortRunningChildren();
                    return State.Failure;
                }

                if (status == State.Running)
                {
                    stillRunning = true;
                }

                childrenLeftToExecute[i] = status;
            }
        }

        return stillRunning ? State.Running : State.Success;
    }

    void AbortRunningChildren()
    {
        for (int i = 0; i < childrenLeftToExecute.Count(); ++i)
        {
            if (childrenLeftToExecute[i] == State.Running)
            {
                Children[i].Abort();
            }
        }
    }
}
