using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Common.DataProcessing.Abstractions;

public interface IThenIncludeBuilder<TEntity, TProperty> : IIncludeBuilder<TEntity>
{
    IThenIncludeBuilder<TEntity, TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, TNextProperty>> thenIncludeExpression)
        where TNextProperty : class, IEntity;
    IThenIncludeBuilder<TEntity, TNextProperty> ThenInclude<TNextProperty>(Expression<Func<TProperty, IEnumerable<TNextProperty>>> thenIncludeExpression)
        where TNextProperty : class, IEntity;
}
