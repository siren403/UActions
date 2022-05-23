using System;
using UActions.Editor.Extensions;
using UnityEngine.UIElements;

namespace UActions.Editor.UI
{
    public class Job : VisualElement
    {
        public string Name
        {
            set => this.Q<Label>(className: "job-name").text = value;
        }

        public event Action Clicked
        {
            add => this.Q<Button>().clicked += value;
            remove => this.Q<Button>().clicked -= value;
        }

        public Job()
        {
            this.AddClassByType<Job>();
            this.AddResource(Bootstrap.Path.GetPath("Editor/UI/Job.uxml"));
        }

        public new class UxmlFactory : UxmlFactory<Job>
        {
        }
    }
}