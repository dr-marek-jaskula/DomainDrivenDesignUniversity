using Quartz;
using Microsoft.Extensions.Options;
using Shopway.Infrastructure.BackgroundJobs;

namespace Shopway.Infrastructure.Options;

public sealed class QuartzOptionsSetup : IConfigureOptions<QuartzOptions>
{
    public void Configure(QuartzOptions options)
    {
        var processOutboxMessageJobKey = JobKey.Create(nameof(ProcessOutboxMessagesJob));

        options
            .AddJob<ProcessOutboxMessagesJob>(jobBuilder => jobBuilder.WithIdentity(processOutboxMessageJobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(processOutboxMessageJobKey)
                    .WithSimpleSchedule(schedule =>
                        schedule
                            .WithIntervalInSeconds(10)
                            .RepeatForever()));

        var deleteOutdatedSoftDeletableEntitiesJobKey = JobKey.Create(nameof(DeleteOutdatedSoftDeletableEntities));

        options
            .AddJob<DeleteOutdatedSoftDeletableEntities>(jobBuilder => jobBuilder.WithIdentity(deleteOutdatedSoftDeletableEntitiesJobKey))
            .AddTrigger(trigger =>
                trigger
                    .ForJob(deleteOutdatedSoftDeletableEntitiesJobKey)
                    .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(5).RepeatForever()));
                    //.WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(1, 23, 0)));
    }
}