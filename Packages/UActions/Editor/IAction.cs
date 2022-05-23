using YamlDotNet.Serialization;

namespace UActions.Editor
{
    public interface IAction
    {
        TargetPlatform Targets { get; }
        void Execute(WorkflowContext context);
    }

    public interface IRegistration
    {
        void Register(DeserializerBuilder builder);
    }
}