namespace UActions.Editor.Actions.iOS
{
    [Obsolete]
    [Action("pbx-project")]
    public class PBXProjectAction : IAction
    {
        public TargetPlatform Targets => TargetPlatform.iOS;

        public void Execute(IWorkflowContext context)
        {
        }
    }
}