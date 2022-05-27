// See https://aka.ms/new-console-template for more information

using ActionsDocExporter;
using UActions.Editor;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var builder = ConsoleApp.CreateBuilder(args);


var app = builder.Build();
app.AddRootCommand(RootCommand);
app.Run();

void RootCommand(string basePath, string destPath, string? actionsDocsPath = null)
{
    var assem = AppDomain.CurrentDomain.GetAssemblies().First(_ => _.GetName().Name == "ActionsDocExporter");
    var actions = assem.GetTypes().Where(_ => _.GetInterface(nameof(IAction)) != null).ToArray();

    var serializer = new SerializerBuilder()
        .WithNamingConvention(HyphenatedNamingConvention.Instance)
        .Build();
    var parser = new ActionParser(serializer, basePath, actionsDocsPath);

    using var writer = new StreamWriter(Path.Combine(basePath, destPath));
    foreach (var action in actions)
    {
        var markdown = parser.ToMarkdown(action);
        writer.Write(markdown);
    }
}