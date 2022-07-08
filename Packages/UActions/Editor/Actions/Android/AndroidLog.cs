using UnityEngine;

namespace UActions.Editor.Actions.Android
{
    public class AndroidLog : IAction
    {
        public TargetPlatform Targets => TargetPlatform.Android;

        public void Execute(IWorkflowContext context)
        {
            if (context.With.TryGetFormat("message", out var value))
            {
                Debug.Log(value);
            }
        }
    }
}