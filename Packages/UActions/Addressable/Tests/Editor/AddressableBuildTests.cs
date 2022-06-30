using NUnit.Framework;
using UActions.Addressable.Editor;
using UActions.Editor;

namespace UActions.Addressable.Tests.Editor
{
    public class AddressableBuildTests
    {
        [Test]
        public void ExecuteTest()
        {
            var runner = WorkflowRunnerBuilder.Fluent()
                .Action<AddressableBuild>()
                .Work("addressable")
                .Step("addressable-build", Map.Empty)
                .Build("addressable");

            runner.Run();
        }
    }
}