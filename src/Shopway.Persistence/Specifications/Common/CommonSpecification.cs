﻿using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using System.Linq.Expressions;

namespace Shopway.Persistence.Specifications.Common;

internal static partial class CommonSpecification
{
    internal static partial class WithMapping
    {
        internal static SpecificationWithMapping<TEntity, TEntityId, TResponse> Create<TEntity, TEntityId, TResponse>
        (
            IFilter<TEntity>? filter = null,
            Expression<Func<TEntity, bool>>? customFilter = null,
            IList<LikeEntry<TEntity>>? likes = null,
            ISortBy<TEntity>? sortBy = null,
            IMapping<TEntity, TResponse>? mapping = null,
            Expression<Func<TEntity, TResponse>>? mappingExpression = null,
            Action<IIncludeBuilder<TEntity>>? buildIncludes = null
        )
            where TEntityId : struct, IEntityId<TEntityId>
            where TEntity : Entity<TEntityId>
        {

            return SpecificationWithMapping<TEntity, TEntityId, TResponse>.New()
                .AddMapping(mapping)
                .AddMapping(mappingExpression)
                .AddIncludes(buildIncludes)
                .AddFilter(filter)
                .AddFilter(customFilter)
                .AddLikes(likes)
                .AddSortBy(sortBy)
                .AddTag($"Common {typeof(TEntity).Name} query")
                .AsMappingSpecification<TResponse>();
        }
    }
}