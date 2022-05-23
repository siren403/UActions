using UActions.Editor;

namespace UActions
{
    public static class Bootstrap
    {
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
    }
}