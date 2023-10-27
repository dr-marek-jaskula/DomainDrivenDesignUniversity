using Quartz;
using System.Reflection;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class DeleteOutdatedSoftDeletableEntitiesJob : IJob
{
    private readonly ShopwayDbContext _dbContext;
    private readonly ILogger<DeleteOutdatedSoftDeletableEntitiesJob> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteOutdatedSoftDeletableEntitiesJob(ShopwayDbContext dbContext, ILogger<DeleteOutdatedSoftDeletableEntitiesJob> logger, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

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

            await (Task)deleteOutdatedEntitiesMethod.Invoke(this, new object[]
            {
                _dbContext,
                _logger,
                _dateTimeProvider,
                context.CancellationToken
            })!;
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
        IDateTimeProvider dateTimeProvider,
        CancellationToken cancellationToken
    )
        where TEntity : class, IEntity, ISoftDeletable
    {
        var entitiesToDelete = await context
            .Set<TEntity>()
            .Where(x => x.SoftDeleted)
            .Where(x => x.SoftDeletedOn < dateTimeProvider.UtcNow.AddYears(-1))
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
        EventName = $"{nameof(DeleteOutdatedSoftDeletableEntitiesJob)}",
        Level = LogLevel.Warning,
        Message = "{jobName} starts",
        SkipEnabledCheck = true
    )]
    public static partial void LogStartingJob(this ILogger logger, string jobName);

    [LoggerMessage
    (
        EventId = 8,
        EventName = $"{nameof(DeleteOutdatedSoftDeletableEntitiesJob)}",
        Level = LogLevel.Warning,
        Message = "{jobName} job ends",
        SkipEnabledCheck = true
    )]
    public static partial void LogEndingJob(this ILogger logger, string jobName);

    [LoggerMessage
    (
        EventId = 9,
        EventName = $"{nameof(DeleteOutdatedSoftDeletableEntitiesJob)}",
        Level = LogLevel.Warning,
        Message = "Deletes '{Count}' entities of type '{EntityType}'.",
        SkipEnabledCheck = true
    )]
    public static partial void LogDeletingOutdatedSoftDeletedEntities(this ILogger logger, int count, string entityType);
}