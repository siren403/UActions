using System;
using System.Collections.Generic;
using System.IO;
using UActions.Runtime;
using UnityEditor;

namespace UActions.Editor.Actions
{
    [Input("path")]
    [Input("data")]
    public class Injection : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(IWorkflowContext context)
        {
            if (!context.With.TryGetFormat("path", out var path))
            {
                throw new Exception($"{nameof(Injection)} empty path");
            }

            var asset = AssetDatabase.LoadAssetAtPath<InjectableObjectBase>(path);
            if (asset == null)
            {
                throw new FileNotFoundException(path);
            }

            var data = context.With.GetValue<Dictionary<object, object>>("data");

            asset.Inject(data);

            EditorUtility.SetDirty(asset);
#if UNITY_2021_2_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(asset);
#else
            AssetDatabase.SaveAssets();
#endif
        }
    }
}