using UActions.Editor.UI;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class UActionsEditorWindow : EditorWindow
    {
        [MenuItem("UActions/Workflow")]
        private static void ShowWindow()
        {
            var window = GetWindow<UActionsEditorWindow>();
            window.titleContent = new GUIContent("Workflow Viewer");
            window.Show();
        }

        private void CreateGUI()
        {
            rootVisualElement.Add(new WorkflowViewer());
        }
    }
}