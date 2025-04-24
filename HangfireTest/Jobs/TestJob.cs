using HangfireTest.Jobs.Attributes;
using HangfireTest.Jobs.Interfaces;

namespace HangfireTest.Jobs
{
    [Job("*/5 * * * * *", "TestJob")]
    public class TestJob : IJob
    {
        public Task Execute()
        {
            Console.WriteLine("Test job executed.");
            return Task.CompletedTask;
        }
    }
}
