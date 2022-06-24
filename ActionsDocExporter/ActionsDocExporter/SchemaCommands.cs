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
        var definitions = CreateDefinitions(types, out var actionNames);
        schema.Add("definitions", definitions);

        var actions = new Actions("#/definitions", actionNames);

        var platforms = new[]
        {
            "android", "ios"
        };
        var properties = CreateProperties(platforms, actions);
        schema.Add("properties", properties);

        #region Works

        definitions.Add("work-actions", new
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
        });

        properties["groups"] = new
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
        };
        properties["works"] = new
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
        };

        #endregion


        var schemaJson = JsonSerializer.Serialize(schema, new JsonSerializerOptions()
        {
            WriteIndented = true
        });

        using var writer = new StreamWriter(path);
        writer.WriteLine(schemaJson);
        writer.Close();
    }

    Dictionary<string, object> CreateDefinitions(Type[] actions, out string[] actionNames)
    {
        actionNames = actions.Select(ExportUtils.GetActionName).ToArray();
        var definitions = new Dictionary<string, object>
        {
            {
                "action-names", new
                {
                    type = "string",
                    oneOf = actionNames.Select(_ => new Dictionary<string, string>()
                    {
                        {"const", _}
                    })
                }
            },
            {
                "action-def", new
                {
                    type = "object",
                    properties = new Dictionary<string, object>()
                    {
                        {"name", new {type = "string"}},
                        {"uses", new Dictionary<string, string>() {{"$ref", "#/definitions/action-names"}}},
                        {"with", new {type = "object"}},
                    }
                }
            }
        };

        for (int i = 0; i < actions.Length; i++)
        {
            definitions.Add(actionNames[i], CreateAction(actions[i], actionNames[i]));
        }

        object CreateAction(Type type, string name)
        {
            var args = ExportUtils.GetArguments(type);
            var action = new Dictionary<string, object>
            {
                {"$ref", "#/definitions/action-def"},
                {
                    "if", new
                    {
                        properties = new
                        {
                            uses = new Dictionary<string, string> {{"const", name}}
                        },
                        required = new[] {"uses"}
                    }
                }
            };
            if (args.Any())
            {
                action.Add("then", new
                {
                    properties = new
                    {
                        with = new
                        {
                            type = "object",
                            properties = args.ToDictionary(_ => _.Name,
                                _ => new {type = ExportUtils.ValueTypeToString(_)})
                        }
                    },
                    required = new[] {"with"},
                    not = new {required = new[] {"name"}}
                });
            }

            return action;
        }

        return definitions;
    }

    Dictionary<string, object> CreateProperties(string[] platforms, Actions actions)
    {
        return new Dictionary<string, object>()
        {
            {"env", new {type = "object"}},
            {
                "steps", new
                {
                    type = "object",
                    patternProperties = new Dictionary<string, object>()
                    {
                        {
                            "", actions
                        }
                    }
                }
            },
            {
                "jobs", new
                {
                    type = "object",
                    patternProperties = new Dictionary<string, object>()
                    {
                        {
                            "", new
                            {
                                type = "object",
                                properties = new
                                {
                                    platform = new Dictionary<string, object>()
                                    {
                                        {"type", "string"},
                                        {
                                            "enum", platforms
                                        }
                                    },
                                    steps = new
                                    {
                                        type = "array",
                                        items = actions
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };
    }
}