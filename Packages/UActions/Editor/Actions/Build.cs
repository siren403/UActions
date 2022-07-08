using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UActions.Editor.Actions
{
    public class Build : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        private readonly string _path;

        public Build(string path)
        {
            _path = path;
        }


        public void Execute(WorkflowContext context)
        {
            var options = new BuildPlayerOptions
            {
                scenes = GetEnableEditorScenes(),
                target = context.CurrentTargets.Target,
                targetGroup = context.CurrentTargets.TargetGroup,
                locationPathName = ValidatePath(context.Format(_path), context.CurrentTargets.Target),
            };
            
            var report = BuildPipeline.BuildPlayer(options);
            if (!Application.isBatchMode)
            {
                ActionHelper.OpenFolder(report.summary.outputPath);
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