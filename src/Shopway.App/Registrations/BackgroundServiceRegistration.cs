using Quartz;
using Shopway.Infrastructure.Options;
using Shopway.Infrastructure.BackgroundJobs;

namespace Microsoft.Extensions.DependencyInjection;

public static class BackgroundServiceRegistration
{
    public static IServiceCollection RegisterBackgroundServices(this IServiceCollection services)
    {
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        services.AddQuartz(options =>
        {
            options.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.ConfigureOptions<QuartzOptionsSetup>();

        return services;
    }
}
