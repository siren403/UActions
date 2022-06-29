using System.Diagnostics;
using UnityEngine;
using Proc = System.Diagnostics.Process;

namespace UActions.Editor.Actions
{
    public class Fastlane : IAction
    {
        private readonly string _platform;
        private readonly string _lane;
        private readonly string _directory;
        public TargetPlatform Targets => TargetPlatform.All;

        public Fastlane(string platform, string lane, string directory)
        {
            _platform = platform;
            _lane = lane;
            _directory = directory;
        }

        public void Execute(WorkflowContext context)
        {
            if (Application.isBatchMode) return;

            var startInfo = new ProcessStartInfo("powershell.exe")
            {
                WorkingDirectory = context.Format(_directory),
                Arguments = $"fastlane {_platform} {_lane}",
            };
#if UNITY_2021_2_OR_NEWER
            using var process = Proc.Start(startInfo);
            process?.WaitForExit();
#else
            var process = Proc.Start(startInfo);
            process?.WaitForExit();
            process?.Dispose();
#endif
        }
    }
}