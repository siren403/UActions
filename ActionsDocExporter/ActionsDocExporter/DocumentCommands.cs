using ActionsDocExporter;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class DocumentCommands : ConsoleAppBase
{
    [Command("doc")]
    public void Export(string basePath, string destPath, string? actionsDocsPath = null)
    {
        var actions = ExportUtils.GetActions();

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
}