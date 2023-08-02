using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree Tree;

    private void Awake()
    {
        Tree = Tree.Clone();
    }
    private void Update()
    {
        Tree.Update();
    }
}
