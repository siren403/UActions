using UnityEngine;

namespace UActions.Editor.Actions
{
    [Input("message")]
    public class Print : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(IWorkflowContext context)
        {
            Debug.Log(context.With.GetFormat("message"));
        }
    }
}