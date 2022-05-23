using UnityEditor;
using UnityEngine;

namespace UActions.Editor.Actions
{
    public class GetVersion : IAction, IActionOutput
    {
        private const string KeyOutput = "VERSION";
        public TargetPlatform Targets => TargetPlatform.All;
        public string[] Keys => new[] {KeyOutput};

        public void Execute(WorkflowContext context)
        {
            context.SetEnv(KeyOutput, PlayerSettings.bundleVersion);
            Debug.Log($"[{nameof(GetVersion)} - Output] $({KeyOutput})");
        }
    }
}