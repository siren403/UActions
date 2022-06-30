using System.Collections.Generic;

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
            Google.IOSResolver.PodToolExecutionViaShellEnabled = _with.Is("use-shell-pods");
#endif
        }
    }
}