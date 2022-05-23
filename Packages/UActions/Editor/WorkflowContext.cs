namespace UActions.Editor
{
    public class WorkflowContext
    {
        private readonly WorkflowArgumentView _argumentView;

        public BuildTargets CurrentTargets { get; }
        public WorkflowContext(WorkflowArgumentView argumentView, BuildTargets currentTargets)
        {
            _argumentView = argumentView;
            CurrentTargets = currentTargets;
        }

        public string Format(string format) => _argumentView.Format(format);
    }
}