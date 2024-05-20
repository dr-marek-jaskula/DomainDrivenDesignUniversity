using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Persistence.Framework;
using System.Reflection;

namespace Shopway.Persistence.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class DeleteOutdatedSoftDeletableEntitiesJob
(
    ShopwayDbContext dbContext,
    ILogger<DeleteOutdatedSoftDeletableEntitiesJob> logger,
    TimeProvider timeProvider
)
    : IJob
{
    private readonly ShopwayDbContext _dbContext = dbContext;
    private readonly ILogger<DeleteOutdatedSoftDeletableEntitiesJob> _logger = logger;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogStartingJob(nameof(DeleteOutdatedSoftDeletableEntitiesJob));

        var entityTypes = _dbContext
            .Model
            .GetEntityTypes()
            .Select(x => x.ClrType)
            .Where(t => t.Implements<ISoftDeletable>());

        foreach (var entityType in entityTypes)
        {
            //This job is called once per month. Therefore, the performance is not important. We can use MetodInfo and then invoke it
            MethodInfo deleteOutdatedEntitiesMethod = typeof(DeleteOutdatedSoftDeletableEntitiesJob)
                .GetSingleGenericMethod(nameof(DeleteOutdatedEntities), entityType);

            await (Task)deleteOutdatedEntitiesMethod.Invoke(this,
            [
                _dbContext,
                _logger,
                _timeProvider,
                context.CancellationToken
            ])!;
        }

        _logger.LogEndingJob(nameof(DeleteOutdatedSoftDeletableEntitiesJob));

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Delete entities that were soft deleted one year in the past. Scheduler is defined in QuartzOptionsSetup
    /// </summary>
    public static async Task DeleteOutdatedEntities<TEntity>
    (
        ShopwayDbContext context,
        ILogger<DeleteOutdatedSoftDeletableEntitiesJob> logger,
        TimeProvider timeProvider,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity, ISoftDeletable
    {
        var entitiesToDelete = await context
            .Set<TEntity>()
            .Where(x => x.SoftDeleted)
            .Where(x => x.SoftDeletedOn < timeProvider.GetUtcNow().AddYears(-1))
            .ToListAsync(cancellationToken);

        logger.LogDeletingOutdatedSoftDeletedEntities(entitiesToDelete.Count, typeof(TEntity).Name);

        context.RemoveRange(entitiesToDelete);
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 7,
        EventName = $"Start {nameof(DeleteOutdatedSoftDeletableEntitiesJob)}",
        Level = LogLevel.Warning,
        Message = "{jobName} starts",
        SkipEnabledCheck = true
    )]
    public static partial void LogStartingJob(this ILogger logger, string jobName);

    [LoggerMessage
    (
        EventId = 8,
        EventName = $"End {nameof(DeleteOutdatedSoftDeletableEntitiesJob)}",
        Level = LogLevel.Warning,
        Message = "{jobName} job ends",
        SkipEnabledCheck = true
    )]
    public static partial void LogEndingJob(this ILogger logger, string jobName);

    [LoggerMessage
    (
        EventId = 9,
        EventName = $"Use {nameof(DeleteOutdatedSoftDeletableEntitiesJob)}",
        Level = LogLevel.Warning,
        Message = "Deletes '{Count}' entities of type '{EntityType}'.",
        SkipEnabledCheck = true
    )]
    public static partial void LogDeletingOutdatedSoftDeletedEntities(this ILogger logger, int count, string entityType);
}