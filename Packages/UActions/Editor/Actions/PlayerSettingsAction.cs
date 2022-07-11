using System.IO;
using UnityEditor;
using UnityEditor.Presets;
using Object = UnityEngine.Object;

namespace UActions.Editor.Actions
{
    [Action("player-settings")]
    [Input("preset")]
    [Input("company-name")]
    [Input("product-name")]
    [Input("version")]
    public class PlayerSettingsAction : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

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

        public void Execute(IWorkflowContext context)
        {
            if (context.With.TryGetValue("preset", out string preset))
            {
                ApplyPreset(preset);
            }

            if (context.With.TryGetFormat("company-name", out string companyName))
            {
                PlayerSettings.companyName = context.Format(companyName);
            }

            if (context.With.TryGetFormat("product-name", out string productName))
            {
                PlayerSettings.productName = context.Format(productName);
            }

            if (context.With.TryGetFormat("version", out string version))
            {
                PlayerSettings.bundleVersion = version;
            }
        }
    }
}