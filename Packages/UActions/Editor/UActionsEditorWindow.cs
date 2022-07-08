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

        [MenuItem("UActions/Validate")]
        private static void Validate()
        {
            var workflow = Bootstrap.LoadWorkflow();
        }


        [MenuItem("UActions/FromUrl")]
        private static void FromUrl()
        {
            Bootstrap.Url(
                "https://gist.githubusercontent.com/qkrsogusl3/430a80902687a43d8026568530b32852/raw/workflow.yaml",
                "from-url"
            );
        }

        private string _work;

        private void OnGUI()
        {
            _work = EditorGUILayout.TextField("Work", _work);
            if (GUILayout.Button("Run"))
            {
                Bootstrap.Run(_work);
            }
        }

        // #if UNITY_2021_2_OR_NEWER
//         public void OnFocus()
//         {
//             // Debug.Log("focus");
//             var viewer = rootVisualElement.Q<WorkflowViewer>();
//             if (viewer != null)
//             {
//                 viewer.Reload();
//             }
//         }
//
//         private void CreateGUI()
//         {
//             rootVisualElement.Add(new WorkflowViewer());
//         }
// #endif
    }
}