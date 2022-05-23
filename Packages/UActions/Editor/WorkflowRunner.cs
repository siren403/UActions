using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UActions.Editor
{
    [Flags]
    public enum TargetPlatform
    {
        None = 0,
        Android = 1 << 0,
        iOS = 1 << 1,
        All = Int32.MaxValue,
    }

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
        private WorkflowContext _context;

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

            if (!Application.isBatchMode)
            {
                buildTarget = targets.Target;
                buildTargetGroup = targets.TargetGroup;

                //switch platform
                var isSuccess = EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);
                if (!isSuccess) throw new Exception($"[{nameof(UActions)}] SwitchPlatform Failed!");
            }

            _argumentView.Platform = job.platform;
            _argumentView.BuildTarget = buildTarget.ToString();
            _argumentView.BuildTargetGroup = buildTargetGroup.ToString();
            _argumentView.ProjectPath = Directory.GetCurrentDirectory();

            _context = new WorkflowContext(_argumentView, targets);

            foreach (var step in job.steps)
            {
                _actionRunner.Run(_context, step.uses, step.with);
            }
        }
    }
}