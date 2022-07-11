using NUnit.Framework;
using UActions.Editor.Actions;
using UnityEditor;
using Assert = UnityEngine.Assertions.Assert;

namespace UActions.Tests.Editor.Actions
{
    public class GetVersionTests : ActionsTestsBase<GetVersion>
    {
        [Test]
        public void ExecuteTest()
        {
            var runner = Run();
            Assert.AreEqual(PlayerSettings.bundleVersion, runner.View[GetVersion.KeyOutput]);
        }
    }
}