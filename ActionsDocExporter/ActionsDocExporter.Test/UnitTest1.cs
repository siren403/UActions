using System;
using System.IO;
using UActions.Editor.Actions;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ActionsDocExporter.Test;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test1()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(HyphenatedNamingConvention.Instance)
            .Build();

        var parser = new ActionParser(serializer);

        var actions = new[]
        {
            typeof(GetVersion),
            typeof(Print),
            typeof(PlayerSettingsAction),
            typeof(PlayerSettingsAndroid),
        };

        using var writer = new StreamWriter(Path.Combine(
            Directory.GetCurrentDirectory(),
            "Actions.md"));

        foreach (var action in actions)
        {
            var markdown = parser.ToMarkdown(action);
            writer.Write(markdown);
        }
    }

    public struct A
    {
    }

    public class B
    {
    }

    public enum E
    {
    }

    [Fact]
    public void TypeTest()
    {
        Assert.False(typeof(A).IsClass);
        Assert.True(typeof(A).IsValueType);

        Assert.True(typeof(B).IsClass);
        Assert.False(typeof(B).IsValueType);

        Assert.True(typeof(string).IsClass);
        Assert.False(typeof(string).IsValueType);
        Assert.False(typeof(string).IsPrimitive);

        Assert.False(typeof(int).IsClass);
        Assert.True(typeof(int).IsValueType);


        Assert.False(typeof(int[]).IsValueType);
        Assert.True(typeof(int[]).IsClass);
        Assert.True(typeof(int[]).IsArray);

        Assert.True(typeof(E).IsValueType);
        Assert.False(typeof(E).IsPrimitive);
        Assert.False(typeof(E).IsClass);
        Assert.False(typeof(E).IsArray);
    }

    [Fact]
    public void PathTest()
    {
        var current = Directory.GetCurrentDirectory();
        _testOutputHelper.WriteLine(current);
        var path = Path.GetRelativePath("../../", current);
        _testOutputHelper.WriteLine(path);
        
    }
}