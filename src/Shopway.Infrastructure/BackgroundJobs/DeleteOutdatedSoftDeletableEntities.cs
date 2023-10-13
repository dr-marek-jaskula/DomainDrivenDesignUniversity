using Quartz;
using System.Reflection;
using Shopway.Domain.Utilities;
using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Framework;
using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class DeleteOutdatedSoftDeletableEntities : IJob
{
    private readonly ShopwayDbContext _dbContext;
    private readonly ILoggerAdapter<DeleteOutdatedSoftDeletableEntities> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public DeleteOutdatedSoftDeletableEntities(ShopwayDbContext dbContext, ILoggerAdapter<DeleteOutdatedSoftDeletableEntities> logger, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("DeleteOutdatedSoftDeletableEntities job starts");

        var entityTypes = _dbContext
            .Model
            .GetEntityTypes()
            .Select(x => x.ClrType)
            .Where(t => t.Implements<ISoftDeletable>());

        foreach (var entityType in entityTypes)
        {
            MethodInfo deleteOutdatedEntitiesMethod = typeof(DeleteOutdatedSoftDeletableEntities)
                .GetSingleGenericMethod(nameof(DeleteOutdatedEntities), entityType);

            await (Task)deleteOutdatedEntitiesMethod.Invoke(this, new object[]
            {
                _dbContext,
                _logger,
                context.CancellationToken
            })!;
        }

        _logger.LogInformation("DeleteOutdatedSoftDeletableEntities job ends");

        await _dbContext.SaveChangesAsync();
    }

    public static async Task DeleteOutdatedEntities<TEntity>
    (
        ShopwayDbContext context,
        ILoggerAdapter<DeleteOutdatedSoftDeletableEntities> logger,
        CancellationToken cancellationToken
    )
        where TEntity : class, ISoftDeletable
    {
        var entitiesToDelete = await context
            .Set<TEntity>()
            .Where(x => x.SoftDeleted)
            .Where(x => x.SoftDeletedOn < DateTimeOffset.UtcNow.AddYears(1))
            .ToListAsync(cancellationToken);

        logger.LogInformation($"Deletes '{entitiesToDelete.Count}' entities of type '{nameof(TEntity)}'.");

        context.RemoveRange(entitiesToDelete);
    }
}