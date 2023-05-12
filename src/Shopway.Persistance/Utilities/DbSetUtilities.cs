using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions;

namespace Shopway.Persistence.Utilities;

public static class DbSetUtilities
{
    public static TEntity AttachAndReturn<TEntity>(this DbSet<TEntity> dbSet, TEntity? entity)
        where TEntity : class, IEntity
    {
        //Make EF Core track the obtained entity
        return dbSet.Attach(entity!).Entity;
    }
}