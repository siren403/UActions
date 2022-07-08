using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace UActions.Editor.Actions
{
    public class PlayerSettingsAndroid : IAction
    {
        public class Registration : IRegistration
        {
            public void Register(DeserializerBuilder builder)
            {
#if UNITY_2021_2_OR_NEWER
                builder.WithTagMapping(new TagName("!architectures"), typeof(AndroidArchitecture[]));
                builder.WithTagMapping(new TagName("!keystore"), typeof(Keystore));
#else
                builder.WithTagMapping("!architectures", typeof(AndroidArchitecture[]));
                builder.WithTagMapping("!keystore", typeof(Keystore));
#endif
            }
        }

        [Serializable]
        public class Keystore
        {
            public string path;
            public string passwd;
            public string alias;
            public string aliasPasswd;
        }

        public TargetPlatform Targets => TargetPlatform.Android;

        private readonly Dictionary<string, object> _with;

        [Input("package-name", isOptional: true)]
        [Input("architectures", true, typeof(AndroidArchitecture[]), isOptional: true)]
        [Input("keystore", true, typeof(Keystore), isOptional: true)]
        [Input("increment-version-code", type: typeof(bool), isOptional: true)]
        [Input("optimized-frame-pacing", type: typeof(bool), isOptional: true)]
        public PlayerSettingsAndroid(Dictionary<string, object> with)
        {
            _with = with;
        }


        public void Execute(IWorkflowContext context)
        {
            if (_with.TryGetIsValue("package-name", out string packageName))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, context.Format(packageName));
            }

            if (_with.TryGetIsValue("architectures", out AndroidArchitecture[] architectures))
            {
                if (architectures.Contains(AndroidArchitecture.ARM64))
                {
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
                }

                PlayerSettings.Android.targetArchitectures = architectures
                    .Aggregate((acc, current) => acc | current);
            }

            if (_with.TryGetIsValue("keystore", out Keystore keystore))
            {
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = context.Format(keystore.path);
                PlayerSettings.Android.keystorePass = context.Format(keystore.passwd);
                PlayerSettings.Android.keyaliasName = context.Format(keystore.alias);
                PlayerSettings.Android.keyaliasPass = context.Format(keystore.aliasPasswd);
            }
            else
            {
                PlayerSettings.Android.useCustomKeystore = false;
                PlayerSettings.Android.keystoreName = string.Empty;
                PlayerSettings.Android.keystorePass = string.Empty;
                PlayerSettings.Android.keyaliasName = string.Empty;
                PlayerSettings.Android.keyaliasPass = string.Empty;
            }

            if (_with.Is("increment-version-code"))
            {
                PlayerSettings.Android.bundleVersionCode++;
            }

#if UNITY_2019_4_OR_NEWER
            PlayerSettings.Android.optimizedFramePacing = _with.Is("optimized-frame-pacing");
#endif
        }
    }
}