using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace UActions.Editor.Actions
{
    [Input(InputSkipBuild, type: typeof(bool), isOptional: true)]
    [Input(InputPath)]
    [Input(InputXcodeAppend, isOptional: true)]
    [Input(InputSymbol, true, typeof(AndroidCreateSymbols), isOptional: true)]
    public class Build : IAction
    {
        // todo: struct
        public const string InputSkipBuild = "skip-build";
        public const string InputPath = "path";
        public const string InputXcodeAppend = "xcode-append";
        public const string InputSymbol = "symbol";

        public class Registration : IRegistration
        {
            public void Register(DeserializerBuilder builder)
            {
#if UNITY_2021_2_OR_NEWER
                builder.WithTagMapping(new TagName("!symbol"), typeof(AndroidCreateSymbols));
#else
                builder.WithTagMapping("!symbol", typeof(AndroidCreateSymbols));
#endif
            }
        }


        public TargetPlatform Targets => TargetPlatform.All;

        public void Execute(IWorkflowContext context)
        {
            if (!context.With.TryGetFormat(InputPath, out var path))
            {
                Console.WriteLine("not found build path");
                return;
            }

            Console.WriteLine($"build path: {path}");
            var buildPath = ValidatePath(path, context.CurrentTargets.Target);

            var additional = BuildOptions.None;

#if UNITY_2019_4_OR_NEWER
            if (context.With.Is(InputXcodeAppend) &&
                BuildPipeline.BuildCanBeAppended(context.CurrentTargets.Target, buildPath) == CanAppendBuild.Yes)
            {
#else
            if(context.With.Is(InputXcodeAppend))
            {
#endif
                // xcode append
                additional |= BuildOptions.AcceptExternalModificationsToPlayer;
            }

            EditorUserBuildSettings.androidCreateSymbols =
                context.With.TryGetValue(InputSymbol, out AndroidCreateSymbols symbols)
                    ? symbols
                    : AndroidCreateSymbols.Disabled;

            var options = new BuildPlayerOptions
            {
                scenes = GetEnableEditorScenes(),
                target = context.CurrentTargets.Target,
                targetGroup = context.CurrentTargets.TargetGroup,
                locationPathName = buildPath,
                options = additional,
            };

            if (!context.With.Is(InputSkipBuild))
            {
                var report = BuildPipeline.BuildPlayer(options);
                if (!Application.isBatchMode)
                {
                    ActionHelper.OpenFolder(report.summary.outputPath);
                }
            }
        }

        private string ValidatePath(string path, BuildTarget target)
        {
#if UNITY_2021_2_OR_NEWER
            return target switch
            {
                BuildTarget.Android => Path.ChangeExtension(path,
                    EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk"),
                _ => path
            };
#else
            switch (target)
            {
                case BuildTarget.Android:
                    return Path.ChangeExtension(path,
                        EditorUserBuildSettings.buildAppBundle ? ".aab" : ".apk");
                default:
                    return path;
            }
#endif
        }

        private string[] GetEnableEditorScenes()
        {
            return EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();
        }
    }
}