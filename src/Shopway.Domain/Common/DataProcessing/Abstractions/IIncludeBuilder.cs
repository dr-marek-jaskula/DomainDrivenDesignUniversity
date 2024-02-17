using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IIncludeBuilder<TEntity>
{
    IThenIncludeBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, TProperty>> includeExpression)
        where TProperty : class, IEntity;
    IThenIncludeBuilder<TEntity, TProperty> Include<TProperty>(Expression<Func<TEntity, IEnumerable<TProperty>>> includeExpression)
        where TProperty : class, IEntity;
}