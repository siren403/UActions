using System;
using System.Collections.Generic;
using System.IO;
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

            var actionRunner = new WorkflowActionRunner();

            var deserializerBuilder = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance);

            actionRunner.Registration(deserializerBuilder);

            var deserializer = deserializerBuilder.Build();

            var path = Path.Combine(Directory.GetCurrentDirectory(), _filePath);
            using var reader = new StreamReader(path);
            var workflow = deserializer.Deserialize<Workflow>(reader.ReadToEnd());

            if (workflow.env != null)
            {
                foreach (var pair in workflow.env)
                {
                    envs[pair.Key] = pair.Value;
                }
            }

            return new WorkflowRunner(workflow, argumentView, actionRunner);
        }
    }
}