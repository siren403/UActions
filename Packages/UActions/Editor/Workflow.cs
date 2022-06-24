using System;
using System.Collections.Generic;
using UnityEditor;

namespace UActions.Editor
{
    [Serializable]
    public class Workflow
    {
        public Dictionary<string, string> env;
        public Dictionary<string, string> input;
        public Dictionary<string, Dictionary<string, Dictionary<string, object>>[]> groups;
        public Dictionary<string, Work> works;
    }

    [Serializable]
    public class Work
    {
        public string platform;
        public object[] steps;
    }
}