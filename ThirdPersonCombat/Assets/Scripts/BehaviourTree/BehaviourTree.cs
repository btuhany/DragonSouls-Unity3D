using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu()]
public class BehaviourTree : ScriptableObject
{
    public Node RootNode;
    public Node.State TreeState = Node.State.Running;
    public List<Node> Nodes = new List<Node>();
    public Blackboard blackboard = new Blackboard();
    public Node.State Update()
    {
        if (RootNode.mState == Node.State.Running)
        {
            TreeState = RootNode.Update();
        }
        return TreeState;
    }

    //public Node CreateNode(System.Type type)
    //{
    //    Node node = ScriptableObject.CreateInstance(type) as Node;
    //    node.name = type.Name;
    //    node.Guid = GUID.Generate().ToString();

    //    Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
    //    Nodes.Add(node);

    //    if (!Application.isPlaying)
    //    {
    //        AssetDatabase.AddObjectToAsset(node, this);
    //    }
    //    Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
    //    AssetDatabase.SaveAssets();
    //    return node;
    //}

    //public void DeleteNode(Node node)
    //{
    //    Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
    //    Nodes.Remove(node);
    //    //AssetDatabase.RemoveObjectFromAsset(node);
    //    Undo.DestroyObjectImmediate(node);
    //    AssetDatabase.SaveAssets();
    //}

    //public void AddChild(Node parent, Node child)
    //{
    //    DecoratorNode decorator = parent as DecoratorNode;
    //    if (decorator)
    //    {
    //        Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
    //        decorator.Child = child;
    //        EditorUtility.SetDirty(decorator);
    //    }

    //    RootNode rootNode = parent as RootNode;
    //    if (rootNode)
    //    {
    //        Undo.RecordObject(rootNode, "Behaviour Tree (AddChild)");
    //        rootNode.Child = child;
    //        EditorUtility.SetDirty(rootNode);
    //    }

    //    CompositeNode composite = parent as CompositeNode;
    //    if (composite)
    //    {
    //        Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
    //        composite.Children.Add(child);
    //        EditorUtility.SetDirty(composite);
    //    }
    //}

    //public void RemoveChild(Node parent, Node child)
    //{
    //    DecoratorNode decorator = parent as DecoratorNode;
    //    if (decorator)
    //    {
    //        Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
    //        decorator.Child = null;
    //        EditorUtility.SetDirty(decorator);
    //    }

    //    RootNode rootNode = parent as RootNode;
    //    if (rootNode)
    //    {
    //        Undo.RecordObject(rootNode, "Behaviour Tree (RemoveChild)");
    //        rootNode.Child = null;
    //        EditorUtility.SetDirty(rootNode);
    //    }

    //    CompositeNode composite = parent as CompositeNode;
    //    if (composite)
    //    {
    //        Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
    //        composite.Children.Remove(child);
    //        EditorUtility.SetDirty(composite);
    //    }
    //}

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.Child != null)
        {
            children.Add(decorator.Child);
        }

        RootNode rootNode = parent as RootNode;
        if (rootNode && rootNode.Child != null)
        {
            children.Add(rootNode.Child);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.Children;
        }

        return children;
    }

    public void Traverse(Node node, System.Action<Node> visiter)
    {
        if (node)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }
    }
    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.RootNode = tree.RootNode.Clone();
        tree.Nodes = new List<Node>();
        Traverse(tree.RootNode, (n) => { tree.Nodes.Add(n); });
        return tree;
    }

    public void Bind(AiAgent agent)
    {
        Traverse(RootNode, node =>
        {
            node.tree = this;
            node.agent = agent;
            node.blackboard = blackboard;
        });
    }
}
