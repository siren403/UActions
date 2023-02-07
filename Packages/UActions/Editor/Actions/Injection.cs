using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UActions.Runtime;
using UnityEditor;
using UnityEngine;
using Object = System.Object;

namespace UActions.Editor.Actions
{
    [Input("path")]
    [Input("data", type: typeof(Object))]
    public class Injection : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(IWorkflowContext context)
        {
            if (!context.With.TryGetFormat("path", out var path))
            {
                throw new Exception($"[{nameof(Injection)}] empty path");
            }

            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(path, "asset");
            }

            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);
            if (asset == null)
            {
                throw new FileNotFoundException(path);
            }

            Inject(context, asset);

            EditorUtility.SetDirty(asset);
#if UNITY_2021_2_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(asset);
#else
            AssetDatabase.SaveAssets();
#endif
        }

        private void Inject(IWorkflowContext context, ScriptableObject asset)
        {
            var data = context.With.GetValue<Dictionary<object, object>>("data");
            var fields = asset.GetType().GetFields(
                    BindingFlags.Instance |
                    BindingFlags.Public |
                    BindingFlags.NonPublic)
                .ToDictionary(f => f.Name);

            void LogSuccess<T>(string key, T v) => Log(context, $"set {key} : {v}");

            foreach (var pair in data)
            {
                var key = pair.Key.ToString();
                if (!fields.TryGetValue(key, out var field))
                {
                    Log(context, $"not found field : {key}");
                    continue;
                }

                var value = pair.Value;
                if (value.GetType() == field.FieldType)
                {
                    SetValue(field, value);
                    continue;
                }

                switch (field.FieldType)
                {
                    case { IsEnum: true }:
                        var enumType = field.FieldType;
                        var str = value.ToString();
                        var names = enumType.GetEnumNames();

                        if (names.Contains(str))
                        {
                            var enumValue = Enum.Parse(enumType, str);
                            SetValue(field, enumValue);
                        }

                        break;
                    case { IsPrimitive: true }:
                        var scalar = value.ToString();

                        if (IsInteger(scalar) || IsFloat(scalar))
                        {
                            var n = Convert.ChangeType(scalar, field.FieldType);
                            SetValue(field, n);
                        }

                        break;
                    default:
                        Log(context, $"not matched field type : {field.FieldType}");
                        break;
                }
            }

            bool IsInteger(string input) =>
                Regex.IsMatch(input, @"^-? ( 0 | [1-9] [0-9]* )$", RegexOptions.IgnorePatternWhitespace);

            bool IsFloat(string input) =>
                Regex.IsMatch(input, @"^-? ( 0 | [1-9] [0-9]* ) ( \. [0-9]* )? ( [eE] [-+]? [0-9]+ )?$",
                    RegexOptions.IgnorePatternWhitespace);

            void SetValue(FieldInfo field, object value)
            {
                if (value is string str)
                {
                    field.SetValue(asset, context.Format(str));
                }
                else
                {
                    field.SetValue(asset, value);
                }

                LogSuccess(field.Name, value);
            }
            // bool IsBool(string input)
            // => Regex.IsMatch(input, @"^(true|false)$", RegexOptions.IgnorePatternWhitespace);
        }

        private void Log(IWorkflowContext context, string log) =>
            context.Log($"[{nameof(Injection)}] {log}");
    }
}