using System;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace UActions.Editor.Actions
{
    [Obsolete]
    [Action("command")]
    public class ExecuteShellCommand : IAction
    {
        private readonly string _args;
        private readonly string _workingDirectory;
        public TargetPlatform Targets => TargetPlatform.All;

        public ExecuteShellCommand(string args, string workingDirectory = null)
        {
            _args = args;
            _workingDirectory = workingDirectory;
        }

        public void Execute(IWorkflowContext context)
        {
            var shell = string.Empty;
#if UNITY_EDITOR_WIN
            shell = "powershell.exe";
#elif UNITY_EDITOR_OSX
            // zsh
            shell = "/bin/zsh";
#elif UNITY_EDITOR_LINUX
            // bash
            shell = "/bin/bash";
#endif

            try
            {
                var startInfo = new ProcessStartInfo(shell)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    Arguments = _args,
                    RedirectStandardOutput = true,
                    WorkingDirectory = context.Format(_workingDirectory)
                };

                var process = System.Diagnostics.Process.Start(startInfo);
                if (process == null) return;
                // process.WaitForExit();
                // Debug.Log(process.StandardOutput.ReadToEnd());
                while (!process.StandardOutput.EndOfStream)
                {
                    context.Log(process.StandardOutput.ReadLine());
                }
            }
            catch (InvalidOperationException e)
            {
                Debug.Log(e);
            }
        }
    }
}