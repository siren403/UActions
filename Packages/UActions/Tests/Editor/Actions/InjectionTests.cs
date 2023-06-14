using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RandomFixtureKit;
using UActions.Editor;
using UActions.Editor.Actions;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace UActions.Tests.Editor.Actions
{
    public class InjectionTests : ActionsTestsBase<Injection>
    {
        [Test]
        public void ExecuteTest()
        {
            var assetPath = Bootstrap.Path.Combine(
                "Tests",
                "Editor",
                "Actions",
                "InjectSample.asset"
            );
            var (url, key, number, mode) = FixtureFactory.Create<(string, string, int, BuildTarget)>();

            Run(new Dictionary<object, object>()
            {
                {"path", assetPath},
                {
                    "data", new Map()
                    {
                        {"url", url},
                        {"key", key},
                        {"number", number},
                        {"mode", mode.ToString()}
                    }
                }
            });

            AssetDatabase.Refresh();
            var asset = AssetDatabase.LoadAssetAtPath<InjectionSample>(assetPath);

            Assert.AreEqual(url, asset.url);
            Assert.AreEqual(key, asset.key);
            Assert.AreEqual(number, asset.number);
            Assert.AreEqual(mode, asset.mode);
        }
    }
}