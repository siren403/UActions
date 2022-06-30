using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UActions.Editor
{
    public readonly struct BuildTargets
    {
#if UNITY_2021_2_OR_NEWER
        public static readonly BuildTargets All = new(TargetPlatform.All, BuildTarget.NoTarget,
            BuildTargetGroup.Unknown);
#else
        public static readonly BuildTargets All =
            new BuildTargets(TargetPlatform.All, BuildTarget.NoTarget, BuildTargetGroup.Unknown);
#endif
        public readonly TargetPlatform TargetPlatform;
        public readonly BuildTarget Target;
        public readonly BuildTargetGroup TargetGroup;

        public BuildTargets(TargetPlatform targetPlatform, BuildTarget target, BuildTargetGroup targetGroup)
        {
            TargetPlatform = targetPlatform;
            Target = target;
            TargetGroup = targetGroup;
        }
    }


    public class WorkflowRunner
    {
        private static readonly Dictionary<string, BuildTargets>
            PlatformNameToTargets = new Dictionary<string, BuildTargets>()
            {
                {"android", new BuildTargets(TargetPlatform.Android, BuildTarget.Android, BuildTargetGroup.Android)},
                {"ios", new BuildTargets(TargetPlatform.iOS, BuildTarget.iOS, BuildTargetGroup.iOS)},
            };

        private readonly WorkflowArgumentView _argumentView;
        private readonly WorkflowActionRunner _actionRunner;
        private readonly Workflow _workflow;

        public Workflow Workflow => _workflow;
        public WorkflowArgumentView View => _argumentView;

        public WorkflowRunner(Workflow workflow, WorkflowArgumentView argumentView,
            WorkflowActionRunner actionRunner)
        {
            _argumentView = argumentView;
            _actionRunner = actionRunner;
            _workflow = workflow;
        }

        public void Run()
        {
            var workName = _argumentView.WorkName;
            if (!string.IsNullOrWhiteSpace(workName))
            {
                if (!_workflow.works.TryGetValue(workName, out var work))
                    throw new Exception($"Not Found Work: {workName}");

                RunWork(workName, work);
                return;
            }

            throw new Exception("not processed runner");
        }

        private void RunWork(string workName, Work work)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);

            if (string.IsNullOrEmpty(work.platform) ||
                !PlatformNameToTargets.TryGetValue(work.platform, out var targets))
            {
                var currentTarget = PlatformNameToTargets.First(pair =>
                    pair.Value.Target == EditorUserBuildSettings.activeBuildTarget);
                work.platform = currentTarget.Key;
                targets = currentTarget.Value;
                // throw new Exception($"not found platform: {work.platform}");
            }

            if (Application.isBatchMode && (buildTarget != targets.Target))
            {
                throw new Exception($"{workName} is {work.platform} platform");
            }

            _argumentView.Platform = work.platform;
            _argumentView.BuildTarget = buildTarget.ToString();
            _argumentView.BuildTargetGroup = buildTargetGroup.ToString();
            _argumentView.ProjectPath = Directory.GetCurrentDirectory();
#if UNITY_2021_2_OR_NEWER
            using WorkflowContext context = new WorkflowContext(_argumentView, targets);
#else
            WorkflowContext context = new WorkflowContext(_argumentView, targets);
#endif
            if (work.steps == null)
            {
                throw new Exception($"Not defined steps from ${workName}");
            }

            foreach (var step in work.steps)
            {
                if (step is string groupName && _workflow.groups.TryGetValue(groupName, out var groups))
                {
                    foreach (var group in groups)
                    {
                        var firstAction = group.First();
                        var actionName = firstAction.Key;
                        var withData = firstAction.Value;
                        _actionRunner.Run(context, actionName, withData);
                    }
                }
                else if (step is Dictionary<object, object> action)
                {
                    // TODO: empty data handling
                    var firstAction = action.First();
                    var actionValue = firstAction.Value;
                    if (actionValue == null)
                    {
                        actionValue = new Dictionary<object, object>();
                    }
                    if (actionValue is Dictionary<object, object> data)
                    {
                        string actionName = firstAction.Key.ToString();
                        var withData = data.ToDictionary(_ => _.Key.ToString(), _ => _.Value);
                        _actionRunner.Run(context, actionName, withData);
                    }
                }
            }
#if !UNITY_2021_2_OR_NEWER
            context.Dispose();
#endif
        }
    }
}