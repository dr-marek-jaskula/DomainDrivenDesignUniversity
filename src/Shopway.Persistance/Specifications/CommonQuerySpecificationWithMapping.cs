using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications;

internal sealed class CommonQuerySpecificationWithMapping<TEntity, TEntityId, TResponse> : SpecificationWithMappingBase<TEntity, TEntityId, TResponse>
    where TEntityId : struct, IEntityId
    where TEntity : Entity<TEntityId>
{
    private CommonQuerySpecificationWithMapping() : base()
    {
    }

    internal static SpecificationWithMappingBase<TEntity, TEntityId, TResponse> Create
    (
        IFilter<TEntity>? filter,
        ISortBy<TEntity>? sortBy,
        Expression<Func<TEntity, TResponse>>? select,
        params Expression<Func<TEntity, object>>[] includes
    )
    {
        return new CommonQuerySpecificationWithMapping<TEntity, TEntityId, TResponse>()
            .AddSelect(select)
            .AddIncludes(includes)
            .AddFilter(filter)
            .AddOrder(sortBy)
            .AsMappingSpecification<TEntity, TEntityId, TResponse>();
    }
}