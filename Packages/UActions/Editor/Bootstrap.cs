using Editor;
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

        public static void Run(string jobName)
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvironmentVariables()
                .SetJobName(jobName)
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

        public static void ExportActionsManifest()
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvironmentVariables()
                .Build();
        }
    }
}