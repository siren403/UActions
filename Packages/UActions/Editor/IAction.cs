using YamlDotNet.Serialization;

namespace UActions.Editor
{
    public interface IAction
    {
        TargetPlatform Targets { get; }
        void Execute(IWorkflowContext context);
    }

    public interface IValidatable
    {
        void Validate(IWorkflowContext context);
    }

    public interface IRegistration
    {
        void Register(DeserializerBuilder builder);
    }
}