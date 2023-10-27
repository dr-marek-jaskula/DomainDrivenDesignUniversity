using System.Linq.Expressions;
using Shopway.Domain.BaseTypes;
using Shopway.Persistence.Utilities;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Abstractions;
using Shopway.Domain.Abstractions.Common;

namespace Shopway.Persistence.Specifications.Common;

internal abstract partial class CommonSpecification
{
    internal sealed partial class WithMapping<TEntity, TEntityId, TResponse> : SpecificationWithMappingBase<TEntity, TEntityId, TResponse>
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntity : Entity<TEntityId>
    {
        internal static SpecificationWithMappingBase<TEntity, TEntityId, TResponse> Create
        (
            IFilter<TEntity>? filter = null,
            Expression<Func<TEntity, bool>>? customFilter = null,
            ISortBy<TEntity>? sortBy = null,
            Expression<Func<TEntity, TResponse>>? mapping = null,
            params Expression<Func<TEntity, object>>[] includes
        )
        {
            return new WithMapping<TEntity, TEntityId, TResponse>()
                .AddMapping(mapping)
                .AddIncludes(includes)
                .AddFilter(filter)
                .AddFilter(customFilter)
                .AddSortBy(sortBy)
                .AddTag($"Common {typeof(TEntity).Name} query")
                .AsMappingSpecification<TEntity, TEntityId, TResponse>();
        }
    }
}