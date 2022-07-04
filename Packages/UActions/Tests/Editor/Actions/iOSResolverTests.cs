using NUnit.Framework;
using UActions.Editor;
using UActions.Editor.Actions;

namespace UActions.Tests.Editor.Actions
{
    public class iOSResolverTests
    {
#if IOS_RESOLVER
        [Test]
        public void ExecuteTest()
        {

            var runner = WorkflowRunnerBuilder.Fluent()
                .Action<iOSResolver>()
                .Work("resolve")
                .Step("ios-resolver", new Map()
                {
                    {"use-shell-pod", false},
                    {"link-frameworks-statically", false},
                })
                .Build();

            runner.Run();
            
            var accessor = new iOSResolver.Accessor();
            Assert.AreEqual(accessor.PodToolExecutionViaShellEnabled, false);
            Assert.AreEqual(accessor.PodfileStaticLinkFrameworks, false);
        }
#endif
    }
}