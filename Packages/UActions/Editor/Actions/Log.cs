using UnityEngine;

namespace UActions.Editor.Actions
{
    public class Log : IAction
    {
        private readonly string _message;
        public TargetPlatform Targets => TargetPlatform.All;

        public Log(string message)
        {
            _message = message;
        }

        public void Execute(WorkflowContext context)
        {
            Debug.Log(context.Format(_message));
        }
    }
}