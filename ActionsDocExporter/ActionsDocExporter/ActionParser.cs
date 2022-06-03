using System.Text;
using UActions.Editor.Actions;
using YamlDotNet.Serialization;

namespace ActionsDocExporter;

using System.Reflection;
using UActions.Editor;

public class ActionParser
{
    private readonly ISerializer _serializer;
    private readonly string? _basePath;
    private readonly string? _docsPath;

    public ActionParser(ISerializer serializer)
    {
        _serializer = serializer;
    }

    public ActionParser(ISerializer serializer, string? basePath, string? docsPath = null)
    {
        _serializer = serializer;
        _basePath = basePath;
        _docsPath = docsPath;
    }

    public string ToMarkdown(Type action)
    {
        var markdown = new StringBuilder();

        var actionName = ExportUtils.GetActionName(action);

        string header;
        if (_basePath != null && _docsPath != null &&
            File.Exists(Path.Combine(_basePath, _docsPath, $"{actionName}.md")))
        {
            header = $"[{actionName}]({_docsPath}/{actionName}.md)";
        }
        else
        {
            header = actionName;
        }

        markdown.AppendLine(header);
        markdown.AppendLine("---\n");

        var constructors = action.GetConstructors(BindingFlags.Instance | BindingFlags.Public);

        var root = new Dictionary<object, object>();
        foreach (var info in constructors)
        {
            root.Clear();

            markdown.AppendLine("```yaml");

            root.Add("uses", actionName);

            var inputs = info.GetCustomAttributes<InputAttribute>().ToArray();
            if (inputs.Any())
            {
                var param = new Dictionary<object, object>();
                foreach (var input in inputs)
                {
                    if (string.IsNullOrEmpty(input.Tag) ||
                        input.Type.IsPrimitive ||
                        input.Type.IsArray ||
                        input.Type.IsEnum ||
                        input.Type == typeof(string))
                    {
                        param.Add(input.Name, $"<{input.Type.Name}{(input.IsOptional ? "?" : string.Empty)}>");
                    }
                    else
                    {
                        var tagParam = new Dictionary<string, string>();
                        var tagFields = input.Type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                        foreach (var field in tagFields)
                        {
                            tagParam.Add(field.Name, $"<{field.Name}>");
                        }

                        param.Add(input.Name, tagParam);
                    }
                }

                root.Add("with", param);

                var yaml = _serializer.Serialize(root);
                foreach (var input in inputs)
                {
                    if (string.IsNullOrEmpty(input.Tag)) continue;
                    var name = input.Name.PascalToKebabCase();
                    yaml = yaml.Replace($"{name}:", $"{name}: {input.Tag}");
                }

                markdown.Append(yaml);
            }
            else
            {
                var parameters = info.GetParameters();
                if (parameters.Any())
                {
                    var param = new Dictionary<object, object>();
                    foreach (var parameter in parameters)
                    {
                        var typeName = parameter.ParameterType.Name;
                        var optionalMark = parameter.IsOptional ? "?" : string.Empty;
                        var optionalValue = parameter.IsOptional ? parameter.DefaultValue : string.Empty;

                        param.Add(parameter.Name?.PascalToKebabCase(),
                            $"<{typeName}{optionalMark}{optionalValue}>");
                    }

                    root.Add("with", param);
                }

                markdown.Append(_serializer.Serialize(root));
            }


            markdown.AppendLine("```");

            var executeMethodInfo = action.GetMethod("Execute");
            if (executeMethodInfo == null)
            {
                throw new Exception("not found Execute() method");
            }

            var outputAttrs = executeMethodInfo.GetCustomAttributes<OutputAttribute>().ToArray();
            if (outputAttrs.Any())
            {
                markdown.AppendLine();
                // markdown.AppendLine("Outputs");
                markdown.AppendLine("```");

                foreach (var output in outputAttrs)
                {
                    markdown.AppendLine($"$({output.Key})");
                }

                markdown.AppendLine("```");
            }
        }

        return markdown.ToString();
    }
}