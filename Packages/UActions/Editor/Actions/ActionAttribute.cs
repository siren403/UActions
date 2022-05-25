using System;

namespace UActions.Editor.Actions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ActionAttribute : Attribute
    {
        public string Name { get; }

        public ActionAttribute(string name)
        {
            Name = name;
        }
    }
}