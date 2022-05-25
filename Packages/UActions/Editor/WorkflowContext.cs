using System;
using System.IO;

namespace UActions.Editor
{
    public class WorkflowContext : IDisposable
    {
        private readonly WorkflowArgumentView _argumentView;

        public BuildTargets CurrentTargets { get; }

        private readonly StreamWriter _logWriter;

        public WorkflowContext(WorkflowArgumentView argumentView, BuildTargets currentTargets)
        {
            _argumentView = argumentView;
            CurrentTargets = currentTargets;
        }

        public WorkflowContext(
            WorkflowArgumentView argumentView,
            BuildTargets currentTargets,
            StreamWriter logWriter)
        {
            _argumentView = argumentView;
            CurrentTargets = currentTargets;
            _logWriter = logWriter;
        }

        public string Format(string format) => _argumentView.Format(format);

        public void SetEnv(string key, string value)
        {
            _argumentView[key] = value;
        }

        public void Dispose()
        {
            _logWriter?.Dispose();
        }

        public void WriteLog(string log)
        {
            _logWriter?.WriteLine(log);
        }
    }
}