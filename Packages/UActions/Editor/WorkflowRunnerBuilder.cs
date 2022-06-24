using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace UActions.Editor
{
    public class WorkflowRunnerBuilder
    {
        private string _filePath = "workflow.yaml";
        private bool _enableLoadEnv;
        private string _jobName;
        private string _workName;
        private Dictionary<string, Type> _actions;
        private Workflow _workflow;

        public WorkflowRunnerBuilder LoadEnvironmentVariables()
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

        public WorkflowRunnerBuilder SetJobName(string jobName)
        {
            _jobName = jobName;
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

        public WorkflowRunner Build()
        {
            Dictionary<string, string> envs = null;
            if (_enableLoadEnv)
            {
                envs = DotEnv.Fluent().Copy();
            }
            else
            {
                envs = new Dictionary<string, string>();
            }

            var argumentView = new WorkflowArgumentView(envs);
            if (!string.IsNullOrEmpty(_jobName))
            {
                argumentView.JobName = _jobName;
            }

            if (!string.IsNullOrEmpty(_workName))
            {
                argumentView.WorkName = _workName;
            }

            WorkflowActionRunner actionRunner = null;
            if (_actions == null || !_actions.Any())
                actionRunner = new WorkflowActionRunner();
            else
                actionRunner = new WorkflowActionRunner(_actions);

            var deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance);

            actionRunner.Registration(deserializerBuilder);

            var deserializer = deserializerBuilder.Build();

            var path = Path.Combine(Directory.GetCurrentDirectory(), _filePath);
            using var reader = new StreamReader(path);

            Workflow workflow = null;

            if (_workflow == null)
            {
                workflow = deserializer.Deserialize<Workflow>(reader.ReadToEnd());

                if (workflow.env != null)
                {
                    foreach (var pair in workflow.env)
                    {
                        envs[pair.Key] = pair.Value;
                    }
                }

                if (workflow.input != null)
                {
                    foreach (var pair in workflow.input)
                    {
                        if (!envs.ContainsKey(pair.Key))
                        {
                            envs[pair.Key] = pair.Value;
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
    }
}