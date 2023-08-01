using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    private BehaviourTree _tree;

    private void Awake()
    {
        _tree = ScriptableObject.CreateInstance<BehaviourTree>();
        DebugLogNode log = ScriptableObject.CreateInstance<DebugLogNode>();
        log.Message = "Messaaaage";

        DebugLogNode log2 = ScriptableObject.CreateInstance<DebugLogNode>();
        log2.Message = "Messaaa535325age";

        DebugLogNode log3 = ScriptableObject.CreateInstance<DebugLogNode>();
        log3.Message = "Messaaa23523age";

        WaitNode wait = ScriptableObject.CreateInstance<WaitNode>();
        wait.WaitTime = 2f;

        SequencerNode sequence = ScriptableObject.CreateInstance<SequencerNode>();
        sequence.Children.Add(log);
        sequence.Children.Add(wait);
        sequence.Children.Add(log2);
        sequence.Children.Add(wait);
        sequence.Children.Add(log3);
        sequence.Children.Add(wait);


        RepeatNode loop = ScriptableObject.CreateInstance<RepeatNode>();
        loop.Child = sequence;



        _tree.RootNode = loop;
    }
    private void Update()
    {
        _tree.Update();
    }
}
