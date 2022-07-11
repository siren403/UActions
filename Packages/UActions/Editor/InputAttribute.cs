using System;

namespace UActions.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class InputAttribute : Attribute
    {
        public string Name { get; }
        public string Tag { get; }
        public Type Type { get; }
        public bool IsOptional { get; }

        public InputAttribute(string name, string tag, Type type, bool isOptional = false)
        {
            if (string.IsNullOrEmpty(name)) throw new Exception("require name");
            Name = name;
            Tag = tag.StartsWith("!") ? tag : $"!{tag}";
            Type = type ?? typeof(string);
            IsOptional = isOptional;
        }

        public InputAttribute(string name, bool nameTag = false, Type type = null, bool isOptional = false)
        {
            if (string.IsNullOrEmpty(name)) throw new Exception("require name");
            Name = name;
            Tag = nameTag ? $"!{name.ToLower()}" : null;
            if (nameTag && type == null)
            {
                throw new Exception("require tag matched type");
            }

            Type = type ?? typeof(string);
            IsOptional = isOptional;
        }
    }
}