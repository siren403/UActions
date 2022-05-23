using System;
using System.Collections.Generic;

namespace UActions.Editor
{
    public class WorkflowArgumentView
    {
        private const string KeySelectedJob = "job";
        private const string KeyProjectPath = "PROJECT_PATH";
        private const string KeyPlatform = "PLATFORM";
        private const string KeyBuildTarget = "BUILD_TARGET";
        private const string KeyBuildTargetGroup = "BUILD_TARGET_GROUP";

        public WorkflowArgumentView(Dictionary<string, string> args)
        {
            _args = args;
        }

        private readonly Dictionary<string, string> _args;

        public string JobName
        {
            get => GetArgument(KeySelectedJob);
            set => _args[KeySelectedJob] = value;
        }

        public string ProjectPath
        {
            get => GetArgument(KeyProjectPath);
            set => _args[KeyProjectPath] = value;
        }

        public string Platform
        {
            get => GetArgument(KeyPlatform);
            set => _args[KeyPlatform] = value;
        }

        public string BuildTarget
        {
            get => GetArgument(KeyBuildTarget);
            set => _args[KeyBuildTarget] = value;
        }

        public string BuildTargetGroup
        {
            get => GetArgument(KeyBuildTargetGroup);
            set => _args[KeyBuildTargetGroup] = value;
        }


        private string GetArgument(string key)
        {
            if (_args.TryGetValue(key, out var value))
            {
                return value;
            }

            throw new Exception($"Not Found Argument: {key}");
        }

        public string Format(string format)
        {
            return format.Format(_args);
        }
    }
}