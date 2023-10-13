using Quartz;
using Shopway.Infrastructure.Options;
using Shopway.Infrastructure.BackgroundJobs;

namespace Microsoft.Extensions.DependencyInjection;

public static class BackgroundServiceRegistration
{
    public static IServiceCollection RegisterBackgroundServices(this IServiceCollection services)
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
