using System;
using YamlDotNet.Core;

namespace UActions.Editor.Actions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class OutputAttribute : Attribute
    {
        public string Key { get; }

        public OutputAttribute(string key)
        {
            Key = key;
        }
    }
}