using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UActions.Editor.Extensions;
using UnityEngine;
using UnityEngine.Networking;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Random = UnityEngine.Random;

namespace UActions.Editor
{
    public class Map : Dictionary<object, object>
    {
        public static Map Empty = new Map();
    }

    public class WorkflowRunnerBuilder
    {
        private string _filePath = "workflow.yaml";
        private string _workflowText;
        private bool _enableLoadEnv;
        private string _workName;
        private Dictionary<string, Type> _actions;
        private Workflow _workflow;
        private bool _fromUrl = false;
        private string _workflowUrl;
        private readonly Dictionary<string, string> _env = new Dictionary<string, string>();

        public WorkflowRunnerBuilder LoadEnvFile()
        {
            _enableLoadEnv = true;
            return this;
        }

        public WorkflowRunnerBuilder SetWorkflowFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new NullReferenceException(nameof(filePath));
            }

            _filePath = filePath;
            return this;
        }

        public WorkflowRunnerBuilder SetWorkflowText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new NullReferenceException(nameof(text));
            }

            _workflowText = text;
            return this;
        }

        public WorkflowRunnerBuilder SetWorkName(string workName)
        {
            _workName = workName;
            return this;
        }

        public WorkflowRunnerBuilder SetActions(Dictionary<string, Type> actions)
        {
            _actions = actions;
            return this;
        }

        public WorkflowRunnerBuilder SetWorkflow(Workflow workflow)
        {
            _workflow = workflow;
            return this;
        }


        private string GetWorkflowTextFromUrl(string url)
        {
            var request = WebRequest.CreateHttp($"{url}?t={Random.value}");
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();

            var workflow = string.Empty;
            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

        private string ReadWorkflowText(WorkflowArgumentView args)
        {
            string input = string.Empty;
            var workflowUrl = string.IsNullOrEmpty(_workflowUrl) ? args.WorkflowUrl : _workflowUrl;
            if (!string.IsNullOrEmpty(workflowUrl))
            {
                input = GetWorkflowTextFromUrl(workflowUrl);
                if (Application.isEditor)
                {
                    Debug.Log(workflowUrl);
                    Debug.Log(input);
                }
            }
            else
            {
                input = _workflowText;
            }

            if (string.IsNullOrEmpty(input))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), _filePath);
#if UNITY_2021_2_OR_NEWER
                using var reader = new StreamReader(path);
                input = reader.ReadToEnd();
#else
                var reader = new StreamReader(path);
                input = reader.ReadToEnd();
                reader.Dispose();
#endif
            }

            if (string.IsNullOrEmpty(input))
            {
                throw new Exception("workflow is empty");
            }

            return input;
        }

        public WorkflowRunnerBuilder RequestFromUrl()
        {
            _fromUrl = true;
            return this;
        }

        public WorkflowRunnerBuilder RequestFromUrl(string url)
        {
            _workflowUrl = url;
            return this;
        }

        public WorkflowRunner Build()
        {
            Dictionary<string, string> env = null;
            env = _enableLoadEnv ? DotEnv.Fluent().Copy() : new Dictionary<string, string>();

            foreach (var pair in _env)
            {
                env.Add(pair.Key, pair.Value);
            }

            var argumentView = new WorkflowArgumentView(env);
            if (!string.IsNullOrEmpty(_workName))
            {
                argumentView.WorkName = _workName;
            }

            WorkflowActionRunner actionRunner = null;
            if (_actions == null || !_actions.Any())
                actionRunner = new WorkflowActionRunner();
            else
                actionRunner = new WorkflowActionRunner(_actions);

#if UNITY_2021_2_OR_NEWER
            var deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance);
#else
            var deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(new HyphenatedNamingConvention());
#endif
            actionRunner.Registration(deserializerBuilder);

            var deserializer = deserializerBuilder.Build();


            Workflow workflow = null;

            if (_workflow == null)
            {
                workflow = deserializer.Deserialize<Workflow>(ReadWorkflowText(argumentView));

                if (workflow.env != null)
                {
                    foreach (var pair in workflow.env)
                    {
                        env[pair.Key] = pair.Value;
                    }
                }

                if (workflow.input != null)
                {
                    foreach (var pair in workflow.input)
                    {
                        if (!env.ContainsKey(pair.Key))
                        {
                            env[pair.Key] = pair.Value;
                        }
                    }
                }
            }
            else
            {
                workflow = _workflow;
            }


            return new WorkflowRunner(workflow, argumentView, actionRunner);
        }


        public class FluentBuilder
        {
            private readonly WorkflowRunnerBuilder _builder;
            private readonly Workflow _workflow;
            private Work _currentWork;
            private List<object> _currentSteps;
            private Dictionary<string, Type> _actions;

            public FluentBuilder()
            {
                _builder = new WorkflowRunnerBuilder();
                _workflow = new Workflow();
                _actions = new Dictionary<string, Type>();
            }

            public FluentBuilder Work(string name = "work")
            {
                if (_workflow.works == null)
                {
                    _workflow.works = new Dictionary<string, Work>();
                }

                _currentWork = new Work();
                _workflow.works[name] = _currentWork;
                return this;
            }

            public FluentBuilder Step(string action, Dictionary<object, object> data)
            {
                if (_currentWork == null)
                {
                    throw new Exception("require work");
                }

                if (_currentWork.steps == null)
                {
                    _currentSteps = new List<object>();
                    _currentWork.steps = _currentSteps;
                }

                _currentSteps.Add(new Dictionary<object, object>()
                {
                    {action, data}
                });

                return this;
            }

            public FluentBuilder Step<T>(Dictionary<object, object> data) where T : IAction
            {
                Step(typeof(T).GetActionName(), data);
                return this;
            }

            public FluentBuilder Step(string groupKey)
            {
                if (_currentWork == null)
                {
                    throw new Exception("require work");
                }

                if (_currentWork.steps == null)
                {
                    _currentSteps = new List<object>();
                    _currentWork.steps = _currentSteps;
                }

                _currentSteps.Add(groupKey);

                return this;
            }

            public FluentBuilder Action<T>() where T : IAction
            {
                var actionType = typeof(T);
                _actions.Add(actionType.GetActionName(), actionType);
                return this;
            }

            public FluentBuilder Env(string key, string value)
            {
                _builder._env[key] = value;
                return this;
            }

            public WorkflowRunner Build(string work = null)
            {
                _builder.SetActions(_actions);
                _builder.SetWorkflow(_workflow);
                if (string.IsNullOrEmpty(work))
                {
                    work = _workflow.works.First().Key;
                }

                _builder.SetWorkName(work);

                return _builder.Build();
            }
        }

        public static FluentBuilder Fluent()
        {
            return new FluentBuilder();
        }
    }
}