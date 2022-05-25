using System.Collections.Generic;
using UnityEditor;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace UActions.Editor.Actions
{
    [Action("player-settings-ios")]
    public class PlayerSettingsiOS : IAction
    {
        public class Registration : IRegistration
        {
            public void Register(DeserializerBuilder builder)
            {
                builder.WithTagMapping(new TagName("!iossdkversion"), typeof(iOSSdkVersion));
            }
        }

        public TargetPlatform Targets => TargetPlatform.iOS;
        private readonly Dictionary<string, object> _with;


        [Input("target-sdk", "!iossdkversion", typeof(iOSSdkVersion), isOptional: true)]
        public PlayerSettingsiOS(Dictionary<string, object> with)
        {
            _with = with;
        }

        public void Execute(WorkflowContext context)
        {
            if (_with.TryGetIsValue("target-sdk", out iOSSdkVersion sdkVersion))
            {
                PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            }
        }
    }
}