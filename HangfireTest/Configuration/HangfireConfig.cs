using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace HangfireTest.Configuration
{
    public static class HangfireConfig
    {
        /// <summary>
        /// This method is used to configure Hangfire with a SQL Server storage and custom options.
        /// </summary>
        public static IServiceCollection ConfigureHangfire(this IServiceCollection services, string dbConnectionString)
        {
            services.AddHangfire(globalConfig =>
            {
                globalConfig
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(dbConnectionString);
            });

            return services;
        }

        /// <summary>
        /// This method is used to configure the Hangfire dashboard with custom options
        /// </summary>
        public static IApplicationBuilder ConfigureHangfireDashboard(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                DashboardTitle = "HangfireTest - Jobs",
                DisplayStorageConnectionString = false,

                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter()
                    {
                        User ="admin",
                        Pass = "admin@123",
                    }
                ]
            });

            return app;
        }
    }
}
