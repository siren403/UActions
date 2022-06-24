using System;
using System.Collections.Generic;
using UActions.Editor;
using UnityEditor;
using Xunit;

namespace ActionsDocExporter.Test;

public class ActionRunnerTests
{
    private readonly WorkflowArgumentView _view;
    private readonly WorkflowActionRunner _runner;
    private readonly WorkflowContext _context;

    public ActionRunnerTests()
    {
        _view = new WorkflowArgumentView(new Dictionary<string, string>());
        _context = new WorkflowContext(_view);

        _runner = new WorkflowActionRunner(new Dictionary<string, Type>()
        {
            {nameof(NoConstructorAction).PascalToKebabCase(), typeof(NoConstructorAction)}
        })
        {
            Logger = new Logger()
        };
    }

    [Fact]
    public void NoConstructorActionTest()
    {
        _runner.Run(_context, nameof(NoConstructorAction).PascalToKebabCase());
        Assert.Equal("success", _view["result"]);
    }
}