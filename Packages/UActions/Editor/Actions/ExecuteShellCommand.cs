using System;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace UActions.Editor.Actions
{
    [Action("command")]
    public class ExecuteShellCommand : IAction
    {
        private readonly string _args;
        public TargetPlatform Targets => TargetPlatform.All;

        public ExecuteShellCommand(string args)
        {
            _args = args;
        }

        public void Execute(WorkflowContext context)
        {
            var shell = string.Empty;
#if UNITY_EDITOR_WIN
            shell = "powershell.exe";
#elif UNITY_EDITOR_OSX
            // zsh
#elif UNITY_EDITOR_LINUX
            // bash
#endif

            try
            {
                var startInfo = new ProcessStartInfo(shell)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = _args,
                    RedirectStandardOutput = true
                };

                var process = System.Diagnostics.Process.Start(startInfo);
                if (process == null) return;
                while (!process.StandardOutput.EndOfStream)
                {
                    Debug.Log(process.StandardOutput.ReadLine());
                }
            }
            catch (InvalidOperationException e)
            {
                Debug.Log(e);
            }
        }
    }
}