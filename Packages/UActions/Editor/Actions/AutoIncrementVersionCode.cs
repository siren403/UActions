using UnityEngine;

namespace UActions.Editor.Actions
{
    public class AutoIncrementVersionCode : IAction
    {
        public TargetPlatform Targets => TargetPlatform.iOS | TargetPlatform.Android;

        private readonly int _a;
        private readonly string _b;
        private readonly int _c;

        public AutoIncrementVersionCode(int a, string b, int c = 10)
        {
            _a = a;
            _b = b;
            _c = c;
        }


        public void Execute(WorkflowContext context)
        {
            Debug.Log($"{_a}, {_b}, {_c}");
        }
    }
}