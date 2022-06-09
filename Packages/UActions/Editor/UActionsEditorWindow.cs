using System;
using UActions.Editor.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UActions.Editor
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

        public void OnFocus()
        {
            // Debug.Log("focus");
            var viewer = rootVisualElement.Q<WorkflowViewer>();
            if (viewer != null)
            {
                viewer.Reload();
            }
        }

        private void CreateGUI()
        {
            rootVisualElement.Add(new WorkflowViewer());
        }
    }
}