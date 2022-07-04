using System;
using System.Reflection;

namespace UActions.Editor.Extensions
{
    public static class ActionExtensions
    {
        public static string GetActionName(this Type actionType)
        {
            var attribute = actionType.GetCustomAttribute<ActionAttribute>();
            if (attribute != null)
            {
                return attribute.Name;
            }

            return actionType.Name.PascalToKebabCase();
        }
    }
}