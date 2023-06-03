using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;
using Shopway.Persistence.Abstractions;

namespace Shopway.Persistence.Specifications.Common;

internal abstract partial class CommonSpecification
{
    internal sealed partial class WithMapping<TEntity, TEntityId, TResponse> : SpecificationWithMappingBase<TEntity, TEntityId, TResponse>
        where TEntityId : struct, IEntityId
        where TEntity : Entity<TEntityId>
    {
        internal static SpecificationWithMappingBase<TEntity, TEntityId, TResponse> Create
        (
            IFilter<TEntity>? filter,
            ISortBy<TEntity>? sortBy,
            Expression<Func<TEntity, TResponse>>? select,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            return new WithMapping<TEntity, TEntityId, TResponse>()
                .AddSelect(select)
                .AddIncludes(includes)
                .AddFilter(filter)
                .AddOrder(sortBy)
                .AsMappingSpecification<TEntity, TEntityId, TResponse>();
        }
    }
}