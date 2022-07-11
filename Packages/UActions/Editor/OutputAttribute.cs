using System;
using YamlDotNet.Core;

namespace UActions.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OutputAttribute : Attribute
    {
        public string Key { get; }

        public OutputAttribute(string key)
        {
            Key = key;
        }
    }
}