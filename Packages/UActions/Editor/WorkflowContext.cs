using System;
using System.Collections.Generic;
using System.IO;
using UActions.Editor.Actions;
using UnityEngine;

namespace UActions.Editor
{
    public interface IWith
    {
        bool Is(string key, bool defaultValue = false);
        bool TryGetFormat(string key, out string value);
        string GetFormat(string key, string defaultValue = null);
        bool TryGetValue<T>(string key, out T value);
        T GetValue<T>(string key, T defaultValue = default);
    }

    public class WithDictionaryData : IWith
    {
        private readonly IWorkflowContext _context;
        public Dictionary<string, object> Data { get; set; }

        public WithDictionaryData(IWorkflowContext context)
        {
            _context = context;
        }

        public bool Is(string key, bool defaultValue = false) => Data.Is(key, defaultValue);
        public bool TryGetFormat(string key, out string value) => Data.TryGetFormat(key, _context, out value);
        public bool TryGetValue<T>(string key, out T value) => Data.TryGetIsValue(key, out value);
        public T GetValue<T>(string key, T defaultValue = default) => Data.GetIsValue(key, defaultValue);
        public string GetFormat(string key, string defaultValue = null) => Data.GetFormat(key, _context, defaultValue);
    }

    public interface IEnv
    {
        string this[string key] { get; set; }
        string Get(string key, string defaultValue = default);
    }

    public interface IWorkflowContext
    {
        string Format(string format);
        BuildTargets CurrentTargets { get; }
        void Log(string log);
        void SetEnv(string key, string value);

        IWith With { get; }
        IEnv Env { get; }

        // ILogger Logger { get; }
    }

    public class WorkflowContext : IWorkflowContext, IDisposable
    {
        private readonly WorkflowArgumentView _argumentView;

        public BuildTargets CurrentTargets { get; }

        private readonly StreamWriter _logWriter;

        private readonly WithDictionaryData _with;

        public Dictionary<string, object> WithData
        {
            set => _with.Data = value;
        }

        public IWith With => _with;
        public IEnv Env => _argumentView;

        public WorkflowContext(WorkflowArgumentView argumentView, BuildTargets currentTargets, string logPath = null)
        {
            _argumentView = argumentView;
            CurrentTargets = currentTargets;
            if (!string.IsNullOrEmpty(logPath))
            {
                _logWriter = new StreamWriter(argumentView.Format(logPath));
            }

            _with = new WithDictionaryData(this);
        }

        public WorkflowContext(WorkflowArgumentView argumentView)
        {
            _argumentView = argumentView;
            CurrentTargets = new BuildTargets();
            _with = new WithDictionaryData(this);
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