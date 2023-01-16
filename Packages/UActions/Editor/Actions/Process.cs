using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace UActions.Editor.Actions
{
    [System.Obsolete]
    public class Process : IAction
    {
        private readonly string _fileName;
        public TargetPlatform Targets => TargetPlatform.All;

        public Process(string fileName)
        {
            _fileName = fileName;
        }

        public void Execute(IWorkflowContext context)
        {
            var startInfo = new ProcessStartInfo()
            {
                FileName = _fileName,
                CreateNoWindow = true,
                UseShellExecute = false,
            };

            var process = System.Diagnostics.Process.Start(startInfo);
            if (process == null)
            {
                Debug.LogWarning("process start failed");
                return;
            }

            while (!process.StandardOutput.EndOfStream)
            {
                Debug.Log(process.StandardOutput.ReadLine());
            }
        }
    }
}