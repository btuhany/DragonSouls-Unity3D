using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree Tree;
    public bool stop;
    private void Awake()
    {
        Tree = Tree.Clone();
        Tree.Bind(GetComponent<AiAgent>());
    }
    private void Update()
    {
        if (stop) return;
        Tree.Update();
    }
}
