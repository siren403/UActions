using System;
using System.Collections.Generic;
using NUnit.Framework;
using UActions.Editor;

namespace UActions.Tests.Editor.Actions
{
    public class ActionsTestsBase<T> where T : IAction
    {
        protected WorkflowRunner Run()
        {
            var runner = WorkflowRunnerBuilder.Fluent()
                .Env("ALLOW_INVALID_TARGET", "true")
                .Action<T>()
                .Work()
                .Step<T>(null)
                .Build();
            runner.Run();

            return runner;
        }

        protected WorkflowRunner Run(Dictionary<object, object> data)
        {
            var runner = WorkflowRunnerBuilder.Fluent()
                .Env("ALLOW_INVALID_TARGET", "true")
                .Action<T>()
                .Work()
                .Step<T>(data)
                .Build();
            runner.Run();

            return runner;
        }

        protected WorkflowRunner Run(Dictionary<string, string> env, Dictionary<object, object> data)
        {
            var builder = WorkflowRunnerBuilder.Fluent()
                .Env("ALLOW_INVALID_TARGET", "true");

            if (env != null)
            {
                foreach (var pair in env)
                {
                    builder.Env(pair.Key, pair.Value);
                }
            }

            var runner = builder.Action<T>()
                .Work()
                .Step<T>(data)
                .Build();
            runner.Run();

            return runner;
        }

        protected void ValidateValue(string key, string value, Func<object> getter)
        {
            var data = new Dictionary<object, object>();
            if (!string.IsNullOrEmpty(key))
            {
                data[key] = value;
            }

            Run(data);

            if (!string.IsNullOrEmpty(key))
            {
                Assert.AreEqual(value, getter());
            }
        }

        protected void ValidateValues((string key, object value, Func<object> getter)[] pairs)
        {
            var data = new Dictionary<object, object>();
            foreach (var (key, value, getter) in pairs)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    data[key] = value;
                }
            }

            Run(data);
            foreach (var (key, value, getter) in pairs)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    Assert.AreEqual(value, getter());
                }
            }
        }
    }
}