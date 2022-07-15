using System;
using System.Collections.Generic;
using UnityEditor;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace UActions.Editor.Actions
{
    [Action("player-settings-ios")]
    [Input("identifier", type: typeof(string), isOptional: true)]
    [Input("increment-version-code", type: typeof(bool), isOptional: true)]
    [Input("target-sdk", "!iossdkversion", typeof(iOSSdkVersion), isOptional: true)]
    [Input("ios-version", type: typeof(string), isOptional: true)]
    public class PlayerSettingsiOS : IAction
    {
        public class Registration : IRegistration
        {
            public void Register(DeserializerBuilder builder)
            {
#if UNITY_2021_2_OR_NEWER
                builder.WithTagMapping(new TagName("!iossdkversion"), typeof(iOSSdkVersion));
#else
                builder.WithTagMapping("!iossdkversion", typeof(iOSSdkVersion));
#endif
            }
        }

        public TargetPlatform Targets => TargetPlatform.iOS;

        public void Execute(IWorkflowContext context)
        {
            if (context.With.TryGetFormat("identifier", out string identifier))
            {
                Console.WriteLine($"set app identifier: {identifier}");
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, identifier);
            }
            else
            {
                throw new Exception("require app identifier");
            }

            if (context.With.Is("increment-version-code"))
            {
                var buildNumber = Convert.ToUInt32(PlayerSettings.iOS.buildNumber);
                PlayerSettings.iOS.buildNumber = (buildNumber + 1).ToString();
            }

            PlayerSettings.iOS.sdkVersion = context.With.GetValue("target-sdk", iOSSdkVersion.DeviceSDK);

            if (context.With.TryGetValue("ios-version", out string version))
            {
                PlayerSettings.iOS.targetOSVersionString = version;
            }
            else
            {
                // arm64 supported version
                PlayerSettings.iOS.targetOSVersionString = "12.0";
            }

#if UNITY_2019
            PlayerSettings.SetArchitecture(BuildTargetGroup.iOS, (int) AppleMobileArchitecture.ARM64);
#endif
        }
    }
}