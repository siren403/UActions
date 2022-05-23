using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using UnityEngine;

namespace UActions.Editor.Actions
{
    [Action("player-settings")]
    public class PlayerSettingsAction : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        private readonly string _companyName;
        private readonly string _productName;
        private readonly string _version;
        private readonly string _preset;

        public PlayerSettingsAction(
            string companyName,
            string productName,
            string version = null,
            string preset = null
        )
        {
            _companyName = companyName;
            _productName = productName;
            _version = version;
            _preset = preset;
        }

        private void ApplyPreset(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("Assets"))
                {
                    path = Path.ChangeExtension(path, ".preset");
                    var preset = AssetDatabase.LoadAssetAtPath<Preset>(path);
                    var asset = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/ProjectSettings.asset");
                    preset.ApplyTo(asset);
                }
                else
                {
                    throw new InvalidDataException("starts with 'Assets'");
                }
            }
        }

        public void Execute(WorkflowContext context)
        {
            ApplyPreset(_preset);

            PlayerSettings.companyName = context.Format(_companyName);
            PlayerSettings.productName = context.Format(_productName);
            if (!string.IsNullOrEmpty(_version))
            {
                PlayerSettings.bundleVersion = _version;
            }
        }
    }
}