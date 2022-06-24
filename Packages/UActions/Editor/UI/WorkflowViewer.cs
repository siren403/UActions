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


            var jobsContainer = this.Q(className: "jobs-container");
            jobsContainer.Clear();

            foreach (var pair in workflow.jobs)
            {
                var job = new Job()
                {
                    Name = pair.Key
                };
                job.Clicked += () => Bootstrap.Run(pair.Key);
                jobsContainer.Add(job);
            }

            var worksContainer = this.Q(className: "works-container");
            worksContainer.Clear();

            if (workflow.works == null) return;

            foreach (var pair in workflow.works)
            {
                var job = new Job()
                {
                    Name = pair.Key
                };
                job.Clicked += () => Bootstrap.RunWork(pair.Key);
                worksContainer.Add(job);
            }
        }

        public new class UxmlFactory : UxmlFactory<WorkflowViewer>
        {
        }
    }
}