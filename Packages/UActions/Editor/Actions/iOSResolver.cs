using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UActions.Editor.Actions
{
    [Action("ios-resolver")]
    public class iOSResolver : IAction
    {
        public TargetPlatform Targets => TargetPlatform.iOS;

        private readonly Dictionary<string, object> _with;

        [Input("use-shell-pods", type: typeof(bool))]
        public iOSResolver(Dictionary<string, object> with)
        {
            _with = with;
        }

        public void Execute(WorkflowContext context)
        {
#if IOS_RESOLVER

            var settingsType = typeof(Google.IOSResolverSettingsDialog)
                .GetNestedType("Settings", BindingFlags.NonPublic);

            var constructor = settingsType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                CallingConventions.Standard,
                Type.EmptyTypes,
                null
            );
            var settings = constructor?.Invoke(null);

            if (_with.TryIs("use-shell-pod", out bool useShellPod))
            {
                var podToolExecutionViaShellEnabled = settingsType
                    .GetField(
                        "podToolExecutionViaShellEnabled",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );
                podToolExecutionViaShellEnabled?.SetValue(settings, useShellPod);
                Debug.Log($"{nameof(iOSResolver)} - {podToolExecutionViaShellEnabled} : {useShellPod}");
            }

            var save = settingsType.GetMethod("Save", BindingFlags.Instance | BindingFlags.NonPublic);
            save?.Invoke(settings, null);
#endif
        }
    }
}