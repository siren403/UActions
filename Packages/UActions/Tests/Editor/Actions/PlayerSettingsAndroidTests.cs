using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UActions.Editor;
using UActions.Editor.Actions;
using UnityEditor;
using Assert = UnityEngine.Assertions.Assert;

namespace UActions.Tests.Editor.Actions
{
    public class PlayerSettingsAndroidTests : ActionsTestsBase<PlayerSettingsAndroid>
    {
        [TestCase("com.action.test")]
        public void PackageNameTest(string packageName)
        {
            Run(new Dictionary<object, object>()
            {
                { "package-name", packageName }
            });

            var changePackageName = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
            Assert.AreEqual(packageName, changePackageName);
        }

        [TestCase(new[] { AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64 })]
        [TestCase(new[] { AndroidArchitecture.ARMv7 })]
        [TestCase(new[] { AndroidArchitecture.ARM64 })]
        public void ArchitecturesTest(AndroidArchitecture[] architectures)
        {
            Run(new Dictionary<object, object>()
            {
                { "architectures", architectures }
            });

            var expected = architectures.Aggregate((acc, current) => acc | current);

            Assert.AreEqual(expected, PlayerSettings.Android.targetArchitectures);
        }

        [Test]
        public void KeystoreDebugTest()
        {
            Run(new Dictionary<object, object>()
            {
                { "keystore", false }
            });
            Assert.IsFalse(PlayerSettings.Android.useCustomKeystore);
            Assert.AreEqual(string.Empty, PlayerSettings.Android.keystoreName);
            Assert.AreEqual(string.Empty, PlayerSettings.Android.keystorePass);
            Assert.AreEqual(string.Empty, PlayerSettings.Android.keyaliasName);
            Assert.AreEqual(string.Empty, PlayerSettings.Android.keyaliasPass);
        }

        [TestCase("user.keystore", "000000", "user", "000000")]
        public void KeystoreTest(string path, string passwd, string alias, string aliasPasswd)
        {
            var keystore = new PlayerSettingsAndroid.Keystore()
            {
                path = path,
                passwd = passwd,
                alias = alias,
                aliasPasswd = aliasPasswd
            };


            Run(new Dictionary<object, object>()
            {
                { "keystore", keystore }
            });
            Assert.IsTrue(PlayerSettings.Android.useCustomKeystore);
            Assert.AreEqual(keystore.path, PlayerSettings.Android.keystoreName);
            Assert.AreEqual(keystore.passwd, PlayerSettings.Android.keystorePass);
            Assert.AreEqual(keystore.alias, PlayerSettings.Android.keyaliasName);
            Assert.AreEqual(keystore.aliasPasswd, PlayerSettings.Android.keyaliasPass);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IncrementVersionCodeTest(bool isEnable)
        {
            var initCode = 0;
            PlayerSettings.Android.bundleVersionCode = initCode;

            Run(new Dictionary<object, object>()
            {
                { "increment-version-code", isEnable }
            });

            if (isEnable)
            {
                initCode++;
            }

            Assert.AreEqual(initCode, PlayerSettings.Android.bundleVersionCode);
        }
    }
}