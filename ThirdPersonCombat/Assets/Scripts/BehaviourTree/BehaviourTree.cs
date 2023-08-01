using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node RootNode;
    public Node.State TreeState = Node.State.Running;

    public Node.State Update()
    {
        if (RootNode.mState == Node.State.Running)
        {
            TreeState = RootNode.Update();
        }
        return TreeState;
    }
}
