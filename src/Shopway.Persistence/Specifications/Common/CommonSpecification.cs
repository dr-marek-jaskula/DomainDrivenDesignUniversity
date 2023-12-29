using System.Linq.Expressions;
using Shopway.Persistence.Utilities;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing.Abstractions;

namespace Shopway.Persistence.Specifications.Common;

internal static partial class CommonSpecification
{
    internal static partial class WithMapping
    {
        internal static SpecificationWithMapping<TEntity, TEntityId, TResponse> Create<TEntity, TEntityId, TResponse>
        (
            IFilter<TEntity>? filter = null,
            Expression<Func<TEntity, bool>>? customFilter = null,
            ISortBy<TEntity>? sortBy = null,
            Expression<Func<TEntity, TResponse>>? mapping = null,
            params Expression<Func<TEntity, object>>[] includes
        )
            where TEntityId : struct, IEntityId<TEntityId>
            where TEntity : Entity<TEntityId>
        {
            return SpecificationWithMapping<TEntity, TEntityId, TResponse>.New()
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