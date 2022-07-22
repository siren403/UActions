using System;
using System.Collections.Generic;
using NUnit.Framework;
using UActions.Editor.Actions;
using UnityEditor;
using Assert = UnityEngine.Assertions.Assert;

namespace UActions.Tests.Editor.Actions
{
    public class PlayerSettingsiOSTests : ActionsTestsBase<PlayerSettingsiOS>
    {
        [TestCase("com.action.ios-test")]
        public void IdentifierTest(string identifier)
        {
            ValidateValue("identifier", identifier,
                () => PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS));
        }

        [TestCase("12", false)]
        [TestCase("0", true)]
        public void IncrementBuildNumberTest(string init, bool isEnable)
        {
            PlayerSettings.iOS.buildNumber = init;
            Run(new Dictionary<object, object>()
            {
                {"identifier", "com.ua.sample"},
                {"increment-version-code", isEnable}
            });
            if (isEnable)
            {
                init = (Convert.ToInt32(init) + 1).ToString();
            }

            Assert.AreEqual(init, PlayerSettings.iOS.buildNumber);
        }

        [TestCase(iOSSdkVersion.DeviceSDK)]
        [TestCase(iOSSdkVersion.SimulatorSDK)]
        public void TargetSdkTest(iOSSdkVersion target)
        {
            Run(new Dictionary<object, object>()
            {
                {"identifier", "com.ua.sample"},
                {"target-sdk", target}
            });
        }

        [TestCase("12.0")]
        [TestCase("13.0")]
        public void iOSVersionTest(string version)
        {
            Run(new Dictionary<object, object>()
            {
                {"identifier", "com.ua.sample"},
                {"ios-version", version}
            });
        }
    }
}