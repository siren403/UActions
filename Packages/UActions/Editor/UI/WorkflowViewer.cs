using UActions;
using UnityEditor;
using UnityEngine.UIElements;

namespace UActions.Editor.UI
{
    public class WorkflowViewer : VisualElement
    {
        public WorkflowViewer()
        {
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                Bootstrap.Path.GetPath($"Editor/UI/{nameof(WorkflowViewer)}.uxml")
            );
            uxml.CloneTree(this);

            Reload();
        }

        public void Reload()
        {
            var workflow = Bootstrap.LoadWorkflow();

            var worksContainer = this.Q(className: "works-container");
            worksContainer.Clear();

            if (workflow.works == null) return;

            foreach (var pair in workflow.works)
            {
                var btn = new Button(() =>
                {
                    Bootstrap.Run(pair.Key);
                })
                {
                    text = pair.Key
                };
                worksContainer.Add(btn);
            }
        }

        public new class UxmlFactory : UxmlFactory<WorkflowViewer>
        {
        }
    }
}