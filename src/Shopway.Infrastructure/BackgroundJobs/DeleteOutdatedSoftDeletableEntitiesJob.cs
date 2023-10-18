using Quartz;
using System.Reflection;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class DeleteOutdatedSoftDeletableEntitiesJob : IJob
{
    private readonly ShopwayDbContext _dbContext;
    private readonly ILoggerAdapter<DeleteOutdatedSoftDeletableEntitiesJob> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteOutdatedSoftDeletableEntitiesJob(ShopwayDbContext dbContext, ILoggerAdapter<DeleteOutdatedSoftDeletableEntitiesJob> logger, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogWarning("{DeleteOutdatedSoftDeletableEntitiesJob} starts", nameof(DeleteOutdatedSoftDeletableEntitiesJob));

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

        _logger.LogWarning("{DeleteOutdatedSoftDeletableEntitiesJob} job ends", nameof(DeleteOutdatedSoftDeletableEntitiesJob));

        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Delete entities that were soft deleted one year in the past. Scheduler is defined in QuartzOptionsSetup
    /// </summary>
    public static async Task DeleteOutdatedEntities<TEntity>
    (
        ShopwayDbContext context,
        ILoggerAdapter<DeleteOutdatedSoftDeletableEntitiesJob> logger,
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

        logger.LogWarning("Deletes '{Count}' entities of type '{EntityType}'.", entitiesToDelete.Count, typeof(TEntity));

        context.RemoveRange(entitiesToDelete);
    }
}