// See https://aka.ms/new-console-template for more information

using ActionsDocExporter;
using UActions.Editor.Actions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

var builder = ConsoleApp.CreateBuilder(args);


var app = builder.Build();
app.AddRootCommand(RootCommand);
app.Run();


void RootCommand(string path)
{
    var assem = AppDomain.CurrentDomain.GetAssemblies().First(_ => _.GetName().Name == "ActionsDocExporter");
    var actions = assem.GetTypes().Where(_ => _.GetInterface(nameof(IAction)) != null).ToArray();

    var serializer = new SerializerBuilder()
        .WithNamingConvention(HyphenatedNamingConvention.Instance)
        .Build();
    var parser = new ActionParser(serializer);


    using var writer = new StreamWriter(path);
    foreach (var action in actions)
    {
        var markdown = parser.ToMarkdown(action);
        writer.Write(markdown);
    }
}

