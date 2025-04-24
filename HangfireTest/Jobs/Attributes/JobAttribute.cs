namespace HangfireTest.Jobs.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class JobAttribute : Attribute
    {
        public string Cron { get; }
        public string? JobId { get; }

        public JobAttribute(string cron, string? jobId = null)
        {
            Cron = cron;
            JobId = jobId;
        }
    }
}