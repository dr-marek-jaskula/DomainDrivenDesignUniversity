using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Domain.Common.DataProcessing;

public sealed record IncludeEntry<TEntity>(LambdaExpression Property, Type EntityType, Type PropertyType, Type? PreviousPropertyType, IncludeType IncludeType)
    where TEntity : class, IEntity;

public enum IncludeType
{
    Include = 1,
    ThenInclude = 2
}
