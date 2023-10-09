using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> OnNodeSelected;
    public Node node;
    public Port Input;
    public Port Output;
    public NodeView(Node node) : base("Assets/UnityResources/UIToolkit/NodeView.uxml")
    {
        this.node = node;
        this.title = node.name;
        this.viewDataKey = node.Guid;

        style.left = node.Position.x;
        style.top = node.Position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();

        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.bindingPath = "description";
        descriptionLabel.Bind(new SerializedObject(node));
    }

    private void SetupClasses()
    {
        if (node is ActionNode)
        {
            AddToClassList("action");
        }
        else if (node is CompositeNode)
        {
            AddToClassList("composite");
        }
        else if (node is DecoratorNode)
        {
            AddToClassList("decorator");
        }
        else if (node is RootNode)
        {
            AddToClassList("root");
        }
    }

    private void CreateOutputPorts()
    {
        if (node is ActionNode)
        {
           
        }
        else if (node is CompositeNode)
        {
            Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (Output != null)
        {
            Output.portName = "";
            Output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(Output);
        }
    }

    private void CreateInputPorts()
    {
        if (node is ActionNode)
        {
            Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is CompositeNode)
        {
            Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            
        }

        if (Input != null)
        {
            Input.portName = "";
            Input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(Input);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(node, "Behaviour Tree (Set Position)");
        node.Position.x = newPos.xMin;
        node.Position.y = newPos.yMin;
        EditorUtility.SetDirty(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();
        OnNodeSelected?.Invoke(this);
    }

    public void SortChildren()
    {
        CompositeNode composite = node as CompositeNode;
        if(composite)
        {
            composite.Children.Sort(SortByHorizontalPosition);
        }
    }

    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.Position.x < right.Position.x ? -1 : 1;
    }

    public void UpdateState()
    {
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");

        if (Application.isPlaying)
        {
            switch (node.mState)
            {
                case Node.State.Running:
                    if (node.Started)
                    {
                        AddToClassList("running");
                    }
                    break;
                case Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Node.State.Success:
                    AddToClassList("success");
                    break;
            }
        }

    }
}
