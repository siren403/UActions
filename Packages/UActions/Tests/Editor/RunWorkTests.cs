using System;
using System.Collections.Generic;
using NUnit.Framework;
using UActions.Editor;
using UActions.Editor.Actions;
using UActions.Editor.Extensions;
using Assert = UnityEngine.Assertions.Assert;

namespace UActions.Tests.Editor
{
    public class SuccessAction : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(IWorkflowContext context)
        {
            context.SetEnv(nameof(SuccessAction).PascalToKebabCase(), "success");
        }
    }

    public class RunWorkTests
    {
        [Test]
        public void RunWorkTestsSimplePasses()
        {
            var runner = WorkflowRunnerBuilder.Fluent()
                .Action<SuccessAction>()
                .Work("first")
                .Step("success-action", Map.Empty)
                .Build();

            runner.Run();

            Assert.AreEqual("success", runner.View[nameof(SuccessAction).PascalToKebabCase()]);
        }


        public class WithDataAction : IAction
        {
            public TargetPlatform Targets => TargetPlatform.All;

            public void Execute(IWorkflowContext context)
            {
                context.Env[typeof(WithDataAction).GetActionName()] = context.With.Is("input").ToString();
            }
        }

        [Test]
        public void WithDataTests()
        {
            var runner = WorkflowRunnerBuilder.Fluent()
                .Action<WithDataAction>()
                .Work()
                .Step(typeof(WithDataAction).GetActionName(), new Map()
                {
                    {"input", "true"}
                })
                .Build();

            runner.Run();

            Assert.AreEqual("True", runner.View[typeof(WithDataAction).GetActionName()]);
        }
    }
}