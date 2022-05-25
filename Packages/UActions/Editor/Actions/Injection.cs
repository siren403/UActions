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

        public void Execute(WorkflowContext context)
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
            AssetDatabase.SaveAssetIfDirty(asset);
        }
    }
}