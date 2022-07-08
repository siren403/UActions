using NUnit.Framework;
using UActions.Editor;
using UActions.Editor.Actions.Android;

namespace UActions.Tests.Editor
{
    public class RenewalTests
    {
        [Test]
        public void WithTest()
        {
            WorkflowRunnerBuilder.Fluent()
                .Action<AndroidLog>()
                .Work()
                .Step<AndroidLog>(new Map()
                {
                    {"message", "android log"}
                })
                .Build()
                .Run();
        }
    }
}