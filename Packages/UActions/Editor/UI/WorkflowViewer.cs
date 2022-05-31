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
            var container = this.Q(className: "workflow-container");
            container.Clear();

            var workflow = Bootstrap.LoadWorkflow();
            foreach (var pair in workflow.jobs)
            {
                var job = new Job()
                {
                    Name = pair.Key
                };
                job.Clicked += () => Bootstrap.Run(pair.Key);
                container.Add(job);
            }
        }

        public new class UxmlFactory : UxmlFactory<WorkflowViewer>
        {
        }
    }
}