using System;
using System.Collections.Generic;
using NUnit.Framework;
using UActions.Editor;
using Assert = UnityEngine.Assertions.Assert;

namespace UActions.Tests.Editor
{
    public class SuccessAction : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(WorkflowContext context)
        {
            context.SetEnv(nameof(SuccessAction).PascalToKebabCase(), "success");
        }
    }

    public class RunWorkTests
    {
        [Test]
        public void RunWorkTestsSimplePasses()
        {
            var runner = new WorkflowRunnerBuilder()
                .SetWorkName("first")
                .SetActions(new Dictionary<string, Type>()
                {
                    {nameof(SuccessAction).PascalToKebabCase(), typeof(SuccessAction)}
                })
                .SetWorkflow(new Workflow()
                {
                    works = new Dictionary<string, Work>()
                    {
                        {
                            "first", new Work()
                            {
                                platform = "android",
                                steps = new List<object>
                                {
                                    new Dictionary<string, Dictionary<string, object>>()
                                    {
                                        {nameof(SuccessAction).PascalToKebabCase(), new Dictionary<string, object>()}
                                    }
                                }
                            }
                        }
                    }
                })
                .Build();

            runner.Run();

            Assert.AreEqual("success", runner.View[nameof(SuccessAction).PascalToKebabCase()]);
        }
        
        [Test]
        public void GroupsTests()
        {
            // var runner = new WorkflowRunnerBuilder()
            //     .SetWorkName("first")
            //     .SetActions(new Dictionary<string, Type>()
            //     {
            //         {nameof(SuccessAction).PascalToKebabCase(), typeof(SuccessAction)}
            //     })
            //     .SetWorkflow(new Workflow()
            //     {
            //         works = new Dictionary<string, Work>()
            //         {
            //             {
            //                 "first", new Work()
            //                 {
            //                     platform = "android",
            //                     steps = new object[]
            //                     {
            //                         new Dictionary<string, Dictionary<string, object>>()
            //                         {
            //                             {nameof(SuccessAction).PascalToKebabCase(), new Dictionary<string, object>()}
            //                         }
            //                     }
            //                 }
            //             }
            //         }
            //     })
            //     .Build();
            //
            // runner.Run();
            //
            // Assert.AreEqual("success", runner.View[nameof(SuccessAction).PascalToKebabCase()]);
        }
    }
}