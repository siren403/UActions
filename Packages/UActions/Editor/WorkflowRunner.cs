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
            PlatformNameToTargets = new()
            {
                {"android", new BuildTargets(TargetPlatform.Android, BuildTarget.Android, BuildTargetGroup.Android)},
                {"ios", new BuildTargets(TargetPlatform.iOS, BuildTarget.iOS, BuildTargetGroup.iOS)},
            };

        private readonly WorkflowArgumentView _argumentView;
        private readonly WorkflowActionRunner _actionRunner;
        private readonly Workflow _workflow;

        public Workflow Workflow => _workflow;

        public WorkflowRunner(Workflow workflow, WorkflowArgumentView argumentView,
            WorkflowActionRunner actionRunner)
        {
            _argumentView = argumentView;
            _actionRunner = actionRunner;
            _workflow = workflow;
        }

        public void Run()
        {
            var jobName = _argumentView.JobName;
            if (!_workflow.jobs.TryGetValue(jobName, out var job))
            {
                throw new Exception($"Not Found Job: {jobName}");
            }

            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);

            if (!PlatformNameToTargets.TryGetValue(job.platform, out var targets))
            {
                throw new Exception($"not found platform: {job.platform}");
            }

            if (Application.isBatchMode && (buildTarget != targets.Target))
            {
                throw new Exception($"{jobName} is {job.platform} platform");
            }

            // if (!Application.isBatchMode)
            // {
            //     buildTarget = targets.Target;
            //     buildTargetGroup = targets.TargetGroup;
            //
            //     //switch platform
            //     var isSuccess = EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
            //     if (!isSuccess) throw new Exception($"[{nameof(UActions)}] SwitchPlatform Failed!");
            // }

            _argumentView.Platform = job.platform;
            _argumentView.BuildTarget = buildTarget.ToString();
            _argumentView.BuildTargetGroup = buildTargetGroup.ToString();
            _argumentView.ProjectPath = Directory.GetCurrentDirectory();

            using WorkflowContext context = new WorkflowContext(_argumentView, targets, job.logFile);

            if (job.steps != null)
            {
                foreach (var step in job.steps)
                {
                    var hasName = !string.IsNullOrEmpty(step.name);
                    Step defined = null;
                    var hasDefined = hasName && _workflow.steps.TryGetValue(step.name, out defined);

                    var hasUses = !string.IsNullOrEmpty(step.uses);


                    if (hasName && hasUses)
                    {
                        Debug.LogWarning(
                            $"[{step.name ?? step.uses}] If step has both name and uses, name takes precedence");
                    }

                    if (hasName && hasDefined)
                    {
                        _actionRunner.Run(context, defined.uses, defined.with);
                    }
                    else if (hasUses)
                    {
                        _actionRunner.Run(context, step.uses, step.with);
                    }
                    else
                    {
                        Debug.LogWarning("step is name or uses required");
                    }
                }
            }

            context.Dispose();
        }
    }
}