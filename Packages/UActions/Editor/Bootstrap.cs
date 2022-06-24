using UActions.Editor;

namespace UActions
{
    public static class Bootstrap
    {
        public static readonly PackagePath.Symbol Path = new PackagePath.Symbol("com.qkrsogusl3.uactions");

        public static void Run()
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvironmentVariables()
                .Build();
            runner.Run();
        }

        public static void Run(string workName)
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvironmentVariables()
                .SetWorkName(workName)
                .Build();
            runner.Run();
        }

        public static Workflow LoadWorkflow()
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvironmentVariables()
                .Build();
            return runner.Workflow;
        }

    }
}