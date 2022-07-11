using UnityEditor;
using UnityEngine;

namespace UActions.Editor.Actions
{
    [Output(KeyOutput)]
    public class GetVersion : IAction
    {
        public const string KeyOutput = "VERSION";
        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(IWorkflowContext context)
        {
            context.Env[KeyOutput] = PlayerSettings.bundleVersion;
            // context.SetEnv(KeyOutput, PlayerSettings.bundleVersion);
            Debug.Log($"[{nameof(GetVersion)} - Output] $({KeyOutput})={context.Env[KeyOutput]}");
        }
    }
}