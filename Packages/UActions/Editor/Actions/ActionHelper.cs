using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace UActions.Editor.Actions
{
    public static class ActionHelper
    {
        public static bool TryGetIsValue<T>(this Dictionary<string, object> dictionary, string key, out T value)
        {
            value = default;
            if (dictionary.TryGetValue(key, out var obj) &&
                obj is T matched)
            {
                value = matched;
                return true;
            }

            return false;
        }

        public static bool TryGetFormat(this Dictionary<string, object> dictionary, string key,
            IWorkflowContext context,
            out string value)
        {
            value = default;
            if (dictionary.TryGetIsValue(key, out value))
            {
                value = context.Format(value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    Console.WriteLine($"{key} is empty");
                    return false;
                }
            }
            else
            {
                Console.WriteLine($"not found {key}");
            }

            return value != default;
        }

        public static string GetFormat(this Dictionary<string, object> dictionary,
            string key,
            IWorkflowContext context,
            string defaultValue = null)
        {
            if (dictionary.TryGetFormat(key, context, out var value))
            {
                return value;
            }

            return string.IsNullOrEmpty(defaultValue) ? key : defaultValue;
        }

        public static T GetIsValue<T>(this Dictionary<string, object> dictionary, string key, T defaultValue = default)
        {
            if (dictionary.TryGetIsValue(key, out T value))
            {
                return value;
            }

            return defaultValue;
        }

        public static bool Is(this Dictionary<string, object> dictionary, string key, bool defaultValue = false)
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                if (value is string str)
                {
                    if (Regex.IsMatch(str, "^(true|y|yes|on)$", RegexOptions.IgnoreCase))
                    {
                        return true;
                    }

                    if (Regex.IsMatch(str, "^(false|n|no|off)$", RegexOptions.IgnoreCase))
                    {
                        return false;
                    }

                    throw new FormatException($"The value \"{value}\" is not a valid YAML Boolean");
                }

                if (value is bool boolean)
                {
                    return boolean;
                }
            }

            return defaultValue;
        }

        public static bool TryIs(this Dictionary<string, object> dictionary, string key, out bool value)
        {
            if (dictionary.TryGetValue(key, out var obj))
            {
                var str = obj.ToString();
                if (Regex.IsMatch(str, "^(true|y|yes|on)$", RegexOptions.IgnoreCase))
                {
                    value = true;
                }
                else if (Regex.IsMatch(str, "^(false|n|no|off)$", RegexOptions.IgnoreCase))
                {
                    value = false;
                }
                else
                {
                    throw new FormatException($"The value \"{obj}\" is not a valid YAML Boolean");
                }

                return true;
            }

            value = false;
            return false;
        }

        public static void OpenFolder(string path)
        {
            if (!Application.isBatchMode)
            {
                EditorUtility.RevealInFinder(path);
            }
        }
    }
}