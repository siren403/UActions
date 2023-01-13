// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using ActionsDocExporter;
using UActions.Editor;

var builder = ConsoleApp.CreateBuilder(args);


var app = builder.Build();
// app.AddRootCommand(() =>
// {
//     Console.WriteLine("PlayerSettingsIOS".PascalToKebabCase());
//     Console.WriteLine(typeof(bool).Name);
//     Console.WriteLine(typeof(object).Name);
//     Console.WriteLine(typeof(Array).Name);
//     Console.WriteLine(typeof(string[]).Name);
// });
app.AddCommands<DocumentCommands>();
app.AddCommands<SchemaCommands>();
app.Run();