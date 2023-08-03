using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree Tree;

    private void Awake()
    {
        Tree = Tree.Clone();
        Tree.Bind(GetComponent<AiAgent>());
    }
    private void Update()
    {
        Tree.Update();
    }
}
