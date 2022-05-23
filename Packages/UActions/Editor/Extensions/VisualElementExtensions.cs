using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UActions.Editor.Extensions
{
    public static class VisualElementExtensions
    {
        public static void AddResource(this VisualElement element, string path)
        {
            if (Path.HasExtension(path))
            {
                switch (Path.GetExtension(path))
                {
                    case ".uxml":
                        element.AddVisualTree(path);
                        return;
                    case ".uss":
                        element.AddStyleSheet(path);
                        return;
                }
            }

            element.AddVisualTree(Path.ChangeExtension(path, "uxml"));
            element.AddStyleSheet(Path.ChangeExtension(path, "uss"));
        }


        public static void AddVisualTree(this VisualElement element, string path)
        {
            var tree = LoadVisualTreeAsset(path);
            tree.CloneTree(element);
        }

        private static VisualTreeAsset LoadVisualTreeAsset(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                // throw new ArgumentNullException(nameof(uxml));
                Debug.LogError($"empty uxml: {path}");
                return null;
            }

            if (!Path.HasExtension(path))
            {
                path = Path.ChangeExtension(path, "uxml");
            }

            var tree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            if (tree == null)
            {
                // throw new ArgumentNullException(uxml);
                Debug.LogError($"not found uxml: {path}");
                return null;
            }

            return tree;
        }

        public static void AddStyleSheet(this VisualElement element, string uss)
        {
            if (string.IsNullOrEmpty(uss))
            {
                Debug.LogError($"empty uss: {uss}");
                return;
            }

            if (!Path.HasExtension(uss))
            {
                uss = Path.ChangeExtension(uss, "uss");
            }

            var style = AssetDatabase.LoadAssetAtPath<StyleSheet>(uss);
            if (style == null) Debug.LogWarning($"not found uss: {uss}");
            else element.styleSheets.Add(style);
        }
        
        public static void AddClassByType<T>(this VisualElement element) where T : VisualElement
        {
            element.AddToClassList(typeof(T).Name.PascalToKebabCase());
        }
    }
}