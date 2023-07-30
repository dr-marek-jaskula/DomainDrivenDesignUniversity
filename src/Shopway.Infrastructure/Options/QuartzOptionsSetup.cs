using Quartz;
using Microsoft.Extensions.Options;
using Shopway.Infrastructure.BackgroundJobs;

namespace Shopway.Infrastructure.Options;

public sealed class QuartzOptionsSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var jobKey = JobKey.Create(nameof(ProcessOutboxMessagesJob));

        options
            .AddJob<ProcessOutboxMessagesJob>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(jobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule
                            .WithIntervalInSeconds(10)
                            .RepeatForever()));
    }
}