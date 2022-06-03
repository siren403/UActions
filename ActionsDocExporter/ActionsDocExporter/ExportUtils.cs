using System.Reflection;
using UActions.Editor;

namespace ActionsDocExporter;

public static class ExportUtils
{
    public static Type[] GetActions()
    {
        var assem = AppDomain.CurrentDomain.GetAssemblies().First(_ => _.GetName().Name == "ActionsDocExporter");
        return assem.GetTypes().Where(_ => _.GetInterface(nameof(IAction)) != null).ToArray();
    }

    public static string GetActionName(Type action)
    {
        var actionsAttr = action.GetCustomAttribute<ActionAttribute>();
        return actionsAttr != null ? actionsAttr.Name : action.Name.PascalToKebabCase();
    }

    public static string ValueTypeToString(Type type)
    {
        var str = type.Name switch
        {
            "String" => "string",
            "Int32" => "number",
            "Boolean" => "boolean",
            _ => "object"
        };

        if (type.IsArray)
        {
            str = "array";
        }

        return str;
    }

    public static ActionArgumentInfo[] GetArguments(Type action)
    {
        var argumentsMap = new Dictionary<string, ActionArgumentInfo>();

        var constructors = action.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        foreach (var info in constructors)
        {
            var inputs = info.GetCustomAttributes<InputAttribute>().ToArray();

            var args = inputs.Any()
                ? inputs.Select(_ => new ActionArgumentInfo(_)).ToArray()
                : info.GetParameters().Select(_ => new ActionArgumentInfo(_)).ToArray();

            foreach (var argumentInfo in args)
            {
                argumentsMap[argumentInfo.Name] = argumentInfo;
            }
        }

        return argumentsMap.Values.ToArray();
    }

    public class ActionArgumentInfo
    {
        public InputAttribute? Input { get; } = null;
        public string Name { get; }
        public bool IsOptional { get; }
        public string Tag { get; }
        public Type ValueType { get; }

        public ActionArgumentInfo(InputAttribute inputAttribute)
        {
            Input = inputAttribute;
            Name = Input.Name;
            IsOptional = Input.IsOptional;
            Tag = Input.Tag;
            ValueType = Input.Type;
        }

        public ActionArgumentInfo(ParameterInfo parameterInfo)
        {
            Name = parameterInfo.Name?.PascalToKebabCase() ?? string.Empty;
            IsOptional = parameterInfo.IsOptional;
            Tag = string.Empty;
            ValueType = parameterInfo.ParameterType;
        }
    }
}