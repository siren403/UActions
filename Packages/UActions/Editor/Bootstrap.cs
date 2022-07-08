using UActions.Editor;

namespace UActions
{
    public static class Bootstrap
    {
        public static readonly PackagePath.Symbol Path = new PackagePath.Symbol("com.qkrsogusl3.uactions");

        public static void Run()
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvFile()
                .Build();
            runner.Run();
        }

        public static void Url()
        {
            new WorkflowRunnerBuilder()
                .LoadEnvFile()
                .RequestFromUrl()
                .Build()
                .Run();
        }

        public static void Run(string workName)
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvFile()
                .SetWorkName(workName)
                .Build();
            runner.Run();
        }

        public static void Url(string url, string workName)
        {
            new WorkflowRunnerBuilder()
                .LoadEnvFile()
                .RequestFromUrl(url)
                .SetWorkName(workName)
                .Build()
                .Run();
        }

        public static Workflow LoadWorkflow()
        {
            var runner = new WorkflowRunnerBuilder()
                .LoadEnvFile()
                .Build();
            return runner.Workflow;
        }
    }
}