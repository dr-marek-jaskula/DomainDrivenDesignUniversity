using Microsoft.Extensions.Options;
using Quartz;
using Shopway.Persistence.BackgroundJobs;

namespace Shopway.Persistence.Options;

public sealed class QuartzOptionsSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var processOutboxMessageJobKey = JobKey.Create(nameof(ProcessOutboxMessagesJob));

        options
            .AddJob<ProcessOutboxMessagesJob>(jobBuilder => jobBuilder.WithIdentity(processOutboxMessageJobKey))
            .AddTrigger(trigger => trigger
                .ForJob(processOutboxMessageJobKey)
                .WithSimpleSchedule(schedule => schedule
                    .WithIntervalInSeconds(10)
                    .RepeatForever()));

        var deleteOutdatedSoftDeletableEntitiesJobKey = JobKey.Create(nameof(DeleteOutdatedSoftDeletableEntitiesJob));

        options
            .AddJob<DeleteOutdatedSoftDeletableEntitiesJob>(jobBuilder => jobBuilder.WithIdentity(deleteOutdatedSoftDeletableEntitiesJobKey))
            .AddTrigger(trigger => trigger
                .ForJob(deleteOutdatedSoftDeletableEntitiesJobKey)
                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 23, 0)));
    }
}
