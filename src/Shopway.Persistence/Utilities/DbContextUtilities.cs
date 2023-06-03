using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;

namespace Shopway.Persistence.Utilities;

public static class DbContextUtilities
{
    public static TEntity AttachToChangeTrackerWhenTrackingBehaviorIsDifferentFromNoTracking<TEntity>(this DbContext context, TEntity? entity)
        where TEntity : class, IEntity
    {
        if (context.ChangeTracker.QueryTrackingBehavior is QueryTrackingBehavior.NoTracking)
        {
            return entity!;
        }

        return context.Set<TEntity>().Attach(entity!).Entity;
    }
}