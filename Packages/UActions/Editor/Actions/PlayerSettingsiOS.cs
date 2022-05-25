using System.Collections.Generic;

namespace UActions.Editor.Actions
{
    [Action("player-settings-ios")]
    public class PlayerSettingsiOS : IAction
    {
        private readonly Dictionary<string, object> _with;
        public TargetPlatform Targets => TargetPlatform.iOS;


        public PlayerSettingsiOS(Dictionary<string, object> with)
        {
            _with = with;
        }

        public void Execute(WorkflowContext context)
        {
        }
    }
}