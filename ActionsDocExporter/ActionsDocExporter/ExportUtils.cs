using System.Reflection;
using UActions.Editor;

namespace ActionsDocExporter;

public static class ExportUtils
{
    public static Type[] GetActions()
    {
        var assem = AppDomain.CurrentDomain.GetAssemblies().First(_ => _.GetName().Name == "ActionsDocExporter");
        return assem.GetTypes()
            .Where(_ => _.GetInterface(nameof(IAction)) != null)
            .Where(_ => _.GetCustomAttribute<ObsoleteAttribute>() == null)
            .ToArray();
    }

    public static string GetActionName(Type action)
    {
        var actionsAttr = action.GetCustomAttribute<ActionAttribute>();
        return actionsAttr != null ? actionsAttr.Name : action.Name.PascalToKebabCase();
    }

    public static string ValueTypeToString(ActionArgumentInfo info)
    {
        var str = info.ValueType.Name switch
        {
            "String" => "string",
            "Int32" => "number",
            "Boolean" => "boolean",
            _ => "object"
        };

        if (info.ValueType.IsArray)
        {
            str = "array";
        }

        // if (!string.IsNullOrEmpty(info.Tag))
        // {
        //     str = "string";
        // }

        return str;
    }

    public static ActionArgumentInfo[] GetArguments(Type action)
    {
        var argumentsMap = new Dictionary<string, ActionArgumentInfo>();

        var inputs = action.GetCustomAttributes<InputAttribute>();

        foreach (var input in inputs)
        {
            argumentsMap[input.Name] = new ActionArgumentInfo(input);
        }
        // var constructors = action.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
        // foreach (var info in constructors)
        // {
        //     var inputs = info.GetCustomAttributes<InputAttribute>().ToArray();
        //
        //     var args = inputs.Any()
        //         ? inputs.Select(_ => new ActionArgumentInfo(_)).ToArray()
        //         : info.GetParameters().Select(_ => new ActionArgumentInfo(_)).ToArray();
        //
        //     foreach (var argumentInfo in args)
        //     {
        //         argumentsMap[argumentInfo.Name] = argumentInfo;
        //     }
        // }

        return argumentsMap.Values.ToArray();
    }

    public static object ArgumentToProperty(ActionArgumentInfo arg)
    {
        var type = ValueTypeToString(arg);
        switch (type)
        {
            case "array":
                object items = arg.ValueType.GetElementType() switch
                {
                    {IsEnum: true} element => new Dictionary<string, object>()
                    {
                        {"type", "string"},
                        {
                            "enum", element.GetEnumNames()
                        }
                    },
                    _ => new
                    {
                        type = "string"
                    }
                };

                var arr = new
                {
                    type, items
                };
                return arr;
            default:
                return new {type};
        }
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