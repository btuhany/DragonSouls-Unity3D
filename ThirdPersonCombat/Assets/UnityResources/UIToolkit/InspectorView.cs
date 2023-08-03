using System;
using UnityEngine.UIElements;
using UnityEditor;
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    private Editor editor;
    public InspectorView() { }

    internal void UpdateSelection(NodeView nodeView)
    {
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() => 
        { 
            if (editor.target)
            {
                editor.OnInspectorGUI(); 
            }
        });
        Add(container);
    }
}
