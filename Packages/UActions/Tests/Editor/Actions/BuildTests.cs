using System.Collections.Generic;
using NUnit.Framework;
using UActions.Editor.Actions;
using UnityEditor;

namespace UActions.Tests.Editor.Actions
{
    public class BuildTests : ActionsTestsBase<Build>
    {
        [TestCase(AndroidCreateSymbols.Public)]
        public void SymbolTest(AndroidCreateSymbols symbols)
        {
            Run(new Dictionary<object, object>()
            {
                {"path", "-"},
                {"symbol", symbols},
                {"skip-build", true}
            });

            Assert.AreEqual(symbols, EditorUserBuildSettings.androidCreateSymbols);
        }
    }
}