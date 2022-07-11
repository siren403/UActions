using System;
using System.Collections.Generic;
using NUnit.Framework;
using UActions.Editor.Actions;
using UnityEditor;
using Assert = UnityEngine.Assertions.Assert;

namespace UActions.Tests.Editor.Actions
{
    public class PlayerSettingsTests : ActionsTestsBase<PlayerSettingsAction>
    {
        [TestCase("Assets/PlayerSettings")]
        public void PresetTest(string path)
        {
            Run(new Dictionary<object, object>()
            {
                {"preset", path}
            });
        }

        [TestCase("action-company", "action-product", "1.0.0")]
        public void ValidateTests(string companyName, string productName, string version)
        {
            ValidateValues(new (string key, object value, Func<object> getter)[]
            {
                ("company-name", companyName, () => PlayerSettings.companyName),
                ("product-name", productName, () => PlayerSettings.productName),
                ("version", version, () => PlayerSettings.bundleVersion),
            });
        }

       
    }
}