using System.Collections.Generic;
using NUnit.Framework;
using UActions.Editor.Actions;

namespace UActions.Tests.Editor.Actions
{
    public class PrintTests : ActionsTestsBase<Print>
    {
        [Test]
        public void ExecuteTest()
        {
            Run(new Dictionary<object, object>()
            {
                {"message", "test"}
            });
        }
    }
}