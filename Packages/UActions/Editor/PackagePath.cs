using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class PackagePath
    {
        public class Symbol
        {
            private readonly string _packageName;

            public Symbol(string packageName)
            {
                _packageName = packageName;
            }

            public string Combine(params string[] paths) => PackagePath.Combine(_packageName, paths);
            public string GetPath(string path) => PackagePath.Combine(_packageName, path);
        }

        [Serializable]
        public class PackageDefinition
        {
            public string name;
            public string displayName;
        }

        private static Dictionary<string, (string path, PackageDefinition definition)> _packages;

        public static string GetRootPath(string packageName)
        {
            _packages ??= AssetDatabase.FindAssets("t:TextAsset package", new[] {"Packages"})
                .Select(AssetDatabase.GUIDToAssetPath)
                .Where(_ => Path.GetFileName(_) == "package.json")
                .Select(_ => (path: _, asset: AssetDatabase.LoadAssetAtPath<TextAsset>(_)))
                .Select(_ => (path: Path.GetDirectoryName(_.path),
                    info: JsonUtility.FromJson<PackageDefinition>(_.asset.text)))
                .ToDictionary(_ => _.info.name, _ => _);

            if (_packages.TryGetValue(packageName, out var package))
            {
                return package.path.Replace("\\", "/");
            }

            return null;
        }

        public static string Combine(string packageName, params string[] paths)
        {
            var root = GetRootPath(packageName);
            if (string.IsNullOrEmpty(root))
            {
                throw new NullReferenceException($"{packageName} path not found");
            }

            var combinePaths = new string[paths.Length + 1];
            combinePaths[0] = root;
            paths.CopyTo(combinePaths, 1);
            return Path.Combine(combinePaths).Replace("\\", "/");
        }

        [MenuItem("UActions/Path")]
        public static void GetPath()
        {
            Debug.Log(GetRootPath("com.qkrsogusl3.uactions"));
            Debug.Log(Combine("com.qkrsogusl3.uactions", "Editor", "UI"));
        }
        
        // this.Q<Button>(className: "btn").clicked += () =>
        // {
        //     var path = AssetDatabase
        //         .FindAssets($"t:script {nameof(WorkflowViewer)}")
        //         .Select(AssetDatabase.GUIDToAssetPath)
        //         .First(_ => Path.GetFileName(_) == $"{nameof(WorkflowViewer)}.cs");
        //
        //     while (!string.IsNullOrEmpty(path) && !Path.GetFileName(path).StartsWith("com."))
        //     {
        //         path = Path.GetDirectoryName(path);
        //     }
        //
        //     Debug.Log(Path.GetFileName(path));
        // };
    }
}