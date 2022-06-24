using System;
using System.IO;
using UnityEngine;

namespace UActions.Editor
{
    public class WorkflowContext : IDisposable
    {
        private readonly WorkflowArgumentView _argumentView;

        public BuildTargets CurrentTargets { get; }

        private readonly StreamWriter _logWriter;

        public WorkflowContext(WorkflowArgumentView argumentView, BuildTargets currentTargets, string logPath = null)
        {
            _argumentView = argumentView;
            CurrentTargets = currentTargets;
            if (!string.IsNullOrEmpty(logPath))
            {
                _logWriter = new StreamWriter(argumentView.Format(logPath));
            }
        }

        public WorkflowContext(WorkflowArgumentView argumentView)
        {
            _argumentView = argumentView;
            CurrentTargets = new BuildTargets();
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

        public void Log(string log)
        {
            Debug.Log(log);
            _logWriter?.WriteLine(log);
        }
    }
}