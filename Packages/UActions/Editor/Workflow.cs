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
        public Dictionary<string, Step> steps;
        public Dictionary<string, Job> jobs;
        public Dictionary<string, Work> works;
    }

    [Serializable]
    public class Step
    {
        public string name;
        public string uses;
        public Dictionary<string, object> with;
    }

    [Serializable]
    public class Job
    {
        public string platform;
        public string logFile;
        public Step[] steps;
    }

    [Serializable]
    public class Work
    {
        public string platform;
        public Dictionary<string, Dictionary<string, object>>[] steps;
    }
}