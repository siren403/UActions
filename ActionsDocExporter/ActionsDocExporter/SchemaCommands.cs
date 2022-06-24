using System.Text.Json;
using UActions.Editor;

namespace ActionsDocExporter;

public class SchemaCommands : ConsoleAppBase
{
    class Actions
    {
        public Dictionary<string, string>[] anyOf { get; }

        public Actions(string path, string[] actionsNames)
        {
            anyOf = new Dictionary<string, string>[actionsNames.Length];
            for (int i = 0; i < actionsNames.Length; i++)
            {
                anyOf[i] = new() {{"$ref", $"{path}/{actionsNames[i]}"}};
            }
        }
    }

    [Command("schema")]
    public void CreateJsonSchema(string path)
    {
        var types = ExportUtils.GetActions();

        var schema = new Dictionary<string, object>()
        {
            {"$schema", "http://json-schema.org/draft-07/schema#"},
            {"description", ""},
            {"type", "object"},
        };
        var definitions = new Dictionary<string, object>
        {
            {
                "work-actions", new
                {
                    type = "array",
                    items = new
                    {
                        anyOf = new object[] {new {type = "string"}}.Concat(
                            types.Select(_ =>
                            {
                                var args = ExportUtils.GetArguments(_);
                                return new
                                {
                                    type = "object",
                                    properties = new Dictionary<string, object>()
                                    {
                                        {
                                            ExportUtils.GetActionName(_), new
                                            {
                                                type = "object",
                                                properties = args.ToDictionary(arg => arg.Name,
                                                    arg => new {type = ExportUtils.ValueTypeToString(arg)})
                                            }
                                        }
                                    }
                                };
                            })
                        )
                    }
                }
            }
        };
        schema.Add("definitions", definitions);

        var platforms = new[]
        {
            "android", "ios"
        };
        var properties = new Dictionary<string, object>()
        {
            {"env", new {type = "object"}},
            {"input", new {type = "object"}},
            {
                "groups", new
                {
                    type = "object",
                    patternProperties = new Dictionary<string, object>()
                    {
                        {
                            "", new Dictionary<string, string>()
                            {
                                {"$ref", "#/definitions/work-actions"}
                            }
                        }
                    }
                }
            },
            {
                "works", new
                {
                    type = "object",
                    patternProperties = new Dictionary<string, object>()
                    {
                        {
                            "", new
                            {
                                properties = new
                                {
                                    platform = new Dictionary<string, object>()
                                    {
                                        {"type", "string"},
                                        {
                                            "enum", platforms
                                        }
                                    },
                                    steps = new Dictionary<string, string>()
                                    {
                                        {"$ref", "#/definitions/work-actions"}
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
        schema.Add("properties", properties);

        var schemaJson = JsonSerializer.Serialize(schema, new JsonSerializerOptions()
        {
            WriteIndented = true
        });

        using var writer = new StreamWriter(path);
        writer.WriteLine(schemaJson);
        writer.Close();
    }
}