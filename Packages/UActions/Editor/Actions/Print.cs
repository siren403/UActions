using UnityEngine;

namespace UActions.Editor.Actions
{
    public class Print : IAction
    {
        private readonly string _message;
        public TargetPlatform Targets => TargetPlatform.All;

        public Print(string message)
        {
            _message = message;
        }

        public void Execute(IWorkflowContext context)
        {
            Debug.Log(context.Format(_message));
        }
    }
}