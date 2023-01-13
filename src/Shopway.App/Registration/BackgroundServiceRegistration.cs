using Quartz;
using Shopway.Infrastructure.BackgroundJobs;

namespace Microsoft.Extensions.DependencyInjection;

public static class BackgroundServiceRegistration
{
    public static IServiceCollection RegisterBackgroundServices(this IServiceCollection services)
    {
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule
                                .WithIntervalInSeconds(10)
                                .RepeatForever()));

            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService();

        return services;
    }
}
