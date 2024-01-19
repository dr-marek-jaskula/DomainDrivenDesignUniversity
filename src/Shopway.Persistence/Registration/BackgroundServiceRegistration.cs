using Quartz;
using Shopway.Persistence.BackgroundJobs;
using Shopway.Persistence.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class BackgroundServiceRegistration
{
    internal static IServiceCollection RegisterBackgroundServices(this IServiceCollection services)
    {
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();
        services.AddScoped<IJob, DeleteOutdatedSoftDeletableEntitiesJob>();
        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.ConfigureOptions<QuartzOptionsSetup>();

        return services;
    }
}
