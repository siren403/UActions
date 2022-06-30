using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UActions.Editor;
using UActions.Editor.Actions;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace UActions.Tests.Editor
{
    public class WorkflowTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void EmptyJobNameException()
        {
            var kebab = nameof(EmptyJobNameException).PascalToKebabCase();

            var factory = new WorkflowActionRunner();
        }

        [Test]
        public void Registrations()
        {
            var result = AppDomain.CurrentDomain.GetAssemblies()
                .Where(_ => _.GetName().Name == "UnityLane.Editor")
                .SelectMany(_ => { return _.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IRegistration))); })
                .ToArray();
        }

        public class PersonA
        {
            public string Name;
        }

        public class PersonB
        {
            public int Age;
        }

        [Test]
        public void MultiDoc()
        {
            var yaml = @"
---
name: first
---
age: 19
---
";
            var input = new StringReader(yaml);
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .Build();

            var parser = new Parser(input);

            // Consume the stream start event "manually"
            parser.Consume<StreamStart>();

            if (parser.Accept<DocumentStart>(out var _))
            {
                var pa = deserializer.Deserialize<PersonA>(parser);
            }

            if (parser.Accept<DocumentStart>(out var _))
            {
                var pb = deserializer.Deserialize<PersonB>(parser);
            }
        }

        [Test]
        public void ActionConstructorArgs()
        {
            var type = typeof(AutoIncrementVersionCode);
            var constructor = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .First();
            var instance = constructor.Invoke(new[] {1, "2", Type.Missing});
        }

        [Test]
        public void DictionaryFormatting()
        {
            var dic = new Dictionary<string, string>()
            {
                {"ENV1", "VALUE1"},
                {"ENV2", "VALUE2"},
                {"ENV3", "VALUE3"},
            };

            var str = "{ENV1} - {ENV2} - {ENV3}";
            var result = str.Format(dic);
        }

        [Test]
        public void ChangeExtension()
        {
            var path = "a/b/c";
            path = Path.ChangeExtension(path, ".apk");
            Debug.Log(path);
            Debug.Log(Path.GetExtension(path));
        }
    }
}