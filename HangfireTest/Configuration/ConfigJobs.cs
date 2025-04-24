using Hangfire;
using HangfireTest.Jobs.Attributes;
using HangfireTest.Jobs.Interfaces;
using System.Reflection;

namespace HangfireTest.Configuration
{
    public static class ConfigJobs
    {
        /// <summary>
        /// This method is used to register all jobs in the assembly that implement the IJob interface (Each job is turned into a services.AddScoped<TestJob, TestJob>()).
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection InstantiateJobs(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<Program>()
                .AddClasses(classes => classes.AssignableTo<IJob>())
                .AsSelf()
                .WithScopedLifetime()
            );

            return services;
        }

        /// <summary>
        /// This method is used to register all jobs in the assembly that implement the IJob interface and have the JobAttribute attribute.
        /// </summary>
        public static IApplicationBuilder ScheduleAllJobs(this IApplicationBuilder app)
        {
            var assembly = typeof(Program).Assembly;

            var jobs = assembly
                .GetTypes()
                .Where(t => typeof(IJob).IsAssignableFrom(t) && t.GetCustomAttribute<JobAttribute>() != null)
                .Select(j => new { Type = j, Atributte = j.GetCustomAttribute<JobAttribute>() });

            using var scope = app.ApplicationServices.CreateScope();
            var provider = scope.ServiceProvider;

            var jobManager = provider.GetRequiredService<IRecurringJobManager>();

            foreach (var job in jobs)
            {
                jobManager.AddOrUpdate(
                    job.Atributte?.JobId ?? job.Type.FullName,
                    () => (provider.GetRequiredService(job.Type) as IJob)!.Execute(),
                    job.Atributte?.Cron
                );
            }

            return app;
        }
    }
}