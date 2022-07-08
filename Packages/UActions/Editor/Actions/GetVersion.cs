using UnityEditor;
using UnityEngine;

namespace UActions.Editor.Actions
{
    public class GetVersion : IAction
    {
        private const string KeyOutput = "VERSION";
        public TargetPlatform Targets => TargetPlatform.All;

        [Output(KeyOutput)]
        public void Execute(IWorkflowContext context)
        {
            context.SetEnv(KeyOutput, PlayerSettings.bundleVersion);
            Debug.Log($"[{nameof(GetVersion)} - Output] $({KeyOutput})");
        }
    }
}