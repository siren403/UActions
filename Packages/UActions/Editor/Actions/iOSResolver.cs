using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UActions.Editor.Actions
{
    [Action("ios-resolver")]
    public class iOSResolver : IAction
    {
        public TargetPlatform Targets => TargetPlatform.All;

        private readonly Dictionary<string, object> _with;

        [Input("use-shell-pod", type: typeof(bool))]
        [Input("link-frameworks-statically", type: typeof(bool))]
        public iOSResolver(Dictionary<string, object> with)
        {
            _with = with;
        }

        public void Execute(WorkflowContext context)
        {
#if IOS_RESOLVER
            var accessor = new Accessor();

            if (_with.TryIs("use-shell-pod", out bool useShellPod))
            {
                accessor.PodToolExecutionViaShellEnabled = useShellPod;
                Debug.Log($"{nameof(iOSResolver)} - {accessor.PodToolExecutionViaShellEnabled} : {useShellPod}");
            }

            if (_with.TryIs("link-frameworks-statically", out var statically))
            {
                accessor.PodfileStaticLinkFrameworks = statically;
                Debug.Log($"{nameof(iOSResolver)} - {accessor.PodfileStaticLinkFrameworks} : {statically}");
            }

            accessor.Save();
#endif
        }

#if IOS_RESOLVER
        public class Accessor
        {
            public bool PodToolExecutionViaShellEnabled
            {
                get => (bool) _podToolExecutionViaShellEnabled.GetValue(_settings);
                set => _podToolExecutionViaShellEnabled.SetValue(_settings, value);
            }

            public bool PodfileStaticLinkFrameworks
            {
                get => (bool) _podfileStaticLinkFrameworks.GetValue(_settings);
                set => _podfileStaticLinkFrameworks.SetValue(_settings, value);
            }

            private readonly object _settings;
            private readonly FieldInfo _podToolExecutionViaShellEnabled;
            private readonly FieldInfo _podfileStaticLinkFrameworks;
            private readonly MethodInfo _save;

            public Accessor()
            {
                var settingsType = typeof(Google.IOSResolverSettingsDialog)
                    .GetNestedType("Settings", BindingFlags.NonPublic);

                _podToolExecutionViaShellEnabled = settingsType
                    .GetField(
                        "podToolExecutionViaShellEnabled",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );
                _podfileStaticLinkFrameworks = settingsType
                    .GetField(
                        "podfileStaticLinkFrameworks",
                        BindingFlags.Instance | BindingFlags.NonPublic
                    );

                _save = settingsType.GetMethod("Save", BindingFlags.Instance | BindingFlags.NonPublic);

                var constructor = settingsType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    null,
                    CallingConventions.Standard,
                    Type.EmptyTypes,
                    null
                );
                _settings = constructor?.Invoke(null);
            }

            public void Save()
            {
                _save.Invoke(_settings, null);
            }
        }
#endif
    }
}