using System;
using System.Collections.Generic;
using System.IO;
using UActions.Runtime;
using UnityEditor;

namespace UActions.Editor.Actions
{
    public class Injection : IAction
    {
        private readonly string _path;
        private readonly Dictionary<object, object> _data;
        public TargetPlatform Targets => TargetPlatform.All;

        public Injection(string path, Dictionary<object, object> data)
        {
            _path = path;
            _data = data;
        }

        public void Execute(IWorkflowContext context)
        {
            if (string.IsNullOrEmpty(_path))
            {
                throw new Exception($"{nameof(Injection)} empty path");
            }

            var asset = AssetDatabase.LoadAssetAtPath<InjectableObjectBase>(_path);
            if (asset == null)
            {
                throw new FileNotFoundException(_path);
            }

            asset.Inject(_data);

            EditorUtility.SetDirty(asset);
#if UNITY_2021_2_OR_NEWER
            AssetDatabase.SaveAssetIfDirty(asset);
#else
            AssetDatabase.SaveAssets();
#endif
        }
    }
}