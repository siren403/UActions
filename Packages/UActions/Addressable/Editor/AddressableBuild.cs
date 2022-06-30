using System.Collections.Generic;
using UActions.Editor;
using UnityEngine;
#if ADDRESSABLE
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
#endif

namespace UActions.Addressable.Editor
{
    public class AddressableBuild : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(WorkflowContext context)
        {
#if ADDRESSABLE
            var builder = AddressableAssetSettingsDefaultObject.Settings.ActivePlayerDataBuilder;
            AddressableAssetSettings.CleanPlayerContent(builder);
            AddressableAssetSettings.BuildPlayerContent(out var result);
            Debug.Log($"{nameof(AddressableBuild)} Result : {result.OutputPath}");
#endif
        }
    }
}