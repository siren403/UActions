using UActions.Editor;

namespace ActionsDocExporter.Test;

public class NoConstructorAction : IAction
{
    public TargetPlatform Targets => TargetPlatform.All;

    public void Execute(WorkflowContext context)
    {
        context.SetEnv("result", "success");
    }
}