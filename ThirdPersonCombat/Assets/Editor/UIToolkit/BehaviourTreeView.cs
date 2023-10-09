//using UnityEngine.UIElements;
//using UnityEditor.Experimental.GraphView;
//using UnityEditor;
//using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//public class BehaviourTreeView : GraphView
//{
//    public Action<NodeView> OnNodeSelected;
//    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }
//    private BehaviourTree _tree;

//    public BehaviourTreeView()
//    {
//        Insert(0, new GridBackground());

//        this.AddManipulator(new ContentZoomer());
//        this.AddManipulator(new ContentDragger());
//        this.AddManipulator(new SelectionDragger());
//        this.AddManipulator(new RectangleSelector());

//        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UnityResources/UIToolkit/BehaviourTreeEditor.uss");
//        styleSheets.Add(styleSheet);

//        Undo.undoRedoPerformed += OnUndoRedoPerformed;
//    }

//    private void OnUndoRedoPerformed()
//    {
//        PopulateView(_tree);
//        AssetDatabase.SaveAssets();
//    }

//    NodeView FindNodeView(Node node)
//    {
//        return GetNodeByGuid(node.Guid) as NodeView;
//    }
//    internal void PopulateView(BehaviourTree tree)
//    {
//        _tree = tree;

//        graphViewChanged -= OnGraphViewChanged;
//        DeleteElements(graphElements);
//        graphViewChanged += OnGraphViewChanged;

//        if (tree.RootNode == null)
//        {
//            tree.RootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
//            EditorUtility.SetDirty(_tree);
//            AssetDatabase.SaveAssets();
//        }

//        tree.Nodes.ForEach(n => CreateNodeView(n));

//        tree.Nodes.ForEach(n => 
//        { 
//            var children = tree.GetChildren(n);
//            children.ForEach(c =>
//            {
//                NodeView parentView = FindNodeView(n);
//                NodeView childView = FindNodeView(c);

//                Edge edge = parentView.Output.ConnectTo(childView.Input);
//                AddElement(edge);
//            });
//        });
//    }

//    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
//    {
//        return ports.ToList().Where(endPort =>
//        endPort.direction != startPort.direction &&
//        endPort.node != startPort.node).ToList();
//    }
//    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
//    {
//        if (graphViewChange.elementsToRemove != null)
//        {
//            graphViewChange.elementsToRemove.ForEach(elem =>
//            {
//                NodeView nodeView = elem as NodeView;
//                if (nodeView != null)
//                {
//                    _tree.DeleteNode(nodeView.node);
//                }

//                Edge edge = elem as Edge;
//                if (edge != null)
//                {
//                    NodeView parentView = edge.output.node as NodeView;
//                    NodeView childView = edge.input.node as NodeView;
//                    _tree.RemoveChild(parentView.node, childView.node);
//                }
//            });
//        }

//        if (graphViewChange.edgesToCreate != null)
//        {
//            graphViewChange.edgesToCreate.ForEach(edge =>
//            {
//                NodeView parentView = edge.output.node as NodeView;
//                NodeView childView = edge.input.node as NodeView;
//                _tree.AddChild(parentView.node, childView.node);
//            });
//        }

//        if(graphViewChange.movedElements != null)
//        {
//            nodes.ForEach((n) =>
//            {
//                NodeView view = n as NodeView;
//                view.SortChildren();
//            });
//        }
//        return graphViewChange;
//    }

//    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
//    {
//        //base.BuildContextualMenu(evt);
//        {
//            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
//            foreach (var type in types)
//            {
//                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
//            }
//        }
//        {
//            var types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
//            foreach (var type in types)
//            {
//                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
//            }
//        }
//        {
//            var types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
//            foreach (var type in types)
//            {
//                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
//            }
//        }


//    }

//    void CreateNode(System.Type type)
//    {
//        if (!_tree)
//        {
//            BehaviourTree newtree = ScriptableObject.CreateInstance("BehaviourTree") as BehaviourTree;
//            string path = $"Assets/DEBUGTREE.asset";
//            AssetDatabase.CreateAsset(newtree, path);
//            // AssetDatabase.AddObjectToAsset(newtree, newtree);
//            AssetDatabase.Refresh();
//            EditorUtility.SetDirty(newtree);
//            AssetDatabase.SaveAssets();
//            _tree = newtree;
//        }
//        Node node = _tree.CreateNode(type);

//        CreateNodeView(node);
//    }
//    void CreateNodeView(Node node)
//    {
//        NodeView nodeView = new NodeView(node);
//        nodeView.OnNodeSelected = OnNodeSelected;
//        AddElement(nodeView);
//    }

//    public void UpdateNodeStates()
//    {
//        nodes.ForEach(n =>
//        {
//            NodeView view = n as NodeView;
//            view.UpdateState();
//        });
//    }
//}
