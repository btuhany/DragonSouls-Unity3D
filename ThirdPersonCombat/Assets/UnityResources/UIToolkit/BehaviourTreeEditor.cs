using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public class BehaviourTreeEditor : EditorWindow
{
    BehaviourTreeView _treeView;
    InspectorView _inspectorView;
    IMGUIContainer _blackboardView;

    SerializedObject treeObject;
    SerializedProperty blackboardProperty;

    [MenuItem("BehaviourTreeEditor/Editor ...")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }
        return false;
    }
    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UnityResources/UIToolkit/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);


        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/UnityResources/UIToolkit/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviourTreeView>();
        _inspectorView = root.Q<InspectorView>();
        _blackboardView = root.Q<IMGUIContainer>();
        _blackboardView.onGUIHandler = () =>
        {
            if(Application.isPlaying)
            {
                treeObject.Update();
            }
            EditorGUILayout.PropertyField(blackboardProperty);
            if (Application.isPlaying)
            {
                treeObject.ApplyModifiedProperties();
            }

        };

        _treeView.OnNodeSelected = OnNodeSelectionChanged;
        OnSelectionChange();   
    }
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }


    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }
    private void OnSelectionChange()
    {
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if(!tree)
        {
            if(Selection.activeGameObject)
            {
                BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
                if(runner)
                {
                    tree = runner.Tree;
                }
            }
        }
        if(Application.isPlaying)
        {
            if (tree && _treeView != null)
            {
                _treeView.PopulateView(tree);
            }
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                _treeView.PopulateView(tree);
            }
        }

        if (tree != null && _treeView != null)
        {
            treeObject = new SerializedObject(tree);
            blackboardProperty = treeObject.FindProperty("blackboard");
        }

    }

    void OnNodeSelectionChanged(NodeView node)
    {
        _inspectorView.UpdateSelection(node);
    }

    private void OnInspectorUpdate()
    {
        _treeView?.UpdateNodeStates();
    }
}