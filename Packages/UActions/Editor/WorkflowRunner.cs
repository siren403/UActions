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
        public static readonly BuildTargets All = new(TargetPlatform.All, BuildTarget.NoTarget,
            BuildTargetGroup.Unknown);

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

            var jobName = _argumentView.JobName;
            if (!string.IsNullOrWhiteSpace(jobName))
            {
                if (!_workflow.jobs.TryGetValue(jobName, out var job))
                    throw new Exception($"Not Found Job: {jobName}");

                RunJob(jobName, job);
                return;
            }

            throw new Exception("not processed runner");
        }

        private void RunWork(string workName, Work work)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var buildTargetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);

            if (!PlatformNameToTargets.TryGetValue(work.platform, out var targets))
            {
                throw new Exception($"not found platform: {work.platform}");
            }

            if (Application.isBatchMode && (buildTarget != targets.Target))
            {
                throw new Exception($"{workName} is {work.platform} platform");
            }

            _argumentView.Platform = work.platform;
            _argumentView.BuildTarget = buildTarget.ToString();
            _argumentView.BuildTargetGroup = buildTargetGroup.ToString();
            _argumentView.ProjectPath = Directory.GetCurrentDirectory();

            using WorkflowContext context = new WorkflowContext(_argumentView, targets);

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
                    var firstAction = action.First();
                    if (firstAction.Value is Dictionary<object, object> data)
                    {
                        string actionName = firstAction.Key.ToString();
                        var withData = data.ToDictionary(_ => _.Key.ToString(), _ => _.Value);
                        _actionRunner.Run(context, actionName, withData);
                    }
                }
            }
        }


        private void RunJob(string jobName, Job job)
        {
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