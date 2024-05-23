using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications;
using Shopway.Persistence.Specifications.Common;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;

namespace Shopway.Persistence.Abstractions;

internal abstract class ProxyRepositoryBase<TEntity, TEntityId>(ShopwayDbContext dbContext, Func<TEntityId, Expression<Func<TEntity, bool>>> cursorFilterFactory)
    where TEntity : Entity<TEntityId>
    where TEntityId : struct, IEntityId<TEntityId>
{
    protected readonly ShopwayDbContext _dbContext = dbContext;
    private readonly Func<TEntityId, Expression<Func<TEntity, bool>>> _cursorFilterFactory = cursorFilterFactory;

    public async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        IOffsetPage page,
        CancellationToken cancellationToken,
        IFilter<TEntity>? filter = null,
        IList<LikeEntry<TEntity>>? likes = null,
        ISortBy<TEntity>? sort = null,
        IMapping<TEntity, TResponse>? mapping = null,
        Expression<Func<TEntity, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<TEntity>>? buildIncludes = null
    )
    {
        var specification = CommonSpecification.Create<TEntity, TEntityId, TResponse>
        (
            filter,
            null,
            likes,
            sort,
            mapping,
            mappingExpression,
            buildIncludes: buildIncludes
        );

        return await _dbContext
            .Set<TEntity>()
            .UseSpecification(specification)
            .PageAsync(page, cancellationToken);
    }

    public async Task<(IList<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>
    (
        ICursorPage page,
        CancellationToken cancellationToken,
        IFilter<TEntity>? filter = null,
        IList<LikeEntry<TEntity>>? likes = null,
        ISortBy<TEntity>? sort = null,
        IMapping<TEntity, TResponse>? mapping = null,
        Expression<Func<TEntity, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<TEntity>>? buildIncludes = null
    )
        where TResponse : class, IHasCursor
    {
        var specification = CommonSpecification.Create<TEntity, TEntityId, TResponse>
        (
            filter,
            _cursorFilterFactory(TEntityId.Create(page.Cursor)),
            likes,
            sort,
            mapping,
            mappingExpression,
            buildIncludes: buildIncludes
        );

        return await _dbContext
            .Set<TEntity>()
            .UseSpecification(specification)
            .PageAsync(page, cancellationToken);
    }

    public async Task<TEntity> QueryByIdAsync
    (
        TEntityId entityId,
        CancellationToken cancellationToken,
        Action<IIncludeBuilder<TEntity>>? buildIncludes = null
    )
    {
        Expression<Func<TEntity, bool>> idFilter = entity => entity.Id.Equals(entityId);

        var specification = CommonSpecification.Create<TEntity, TEntityId>(idFilter, buildIncludes);

        return await _dbContext
            .Set<TEntity>()
            .UseSpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<TResponse> QueryByIdAsync<TResponse>
    (
        TEntityId entityId,
        CancellationToken cancellationToken,
        IMapping<TEntity, TResponse>? mapping = null
    )
        where TResponse : class
    {
        Expression<Func<TEntity, bool>> idFilter = entity => entity.Id.Equals(entityId);

        var specificationWithMapping = CommonSpecification.Create<TEntity, TEntityId, TResponse>(idFilter, mapping);

        return await _dbContext
            .Set<TEntity>()
            .UseSpecification(specificationWithMapping)
            .FirstAsync(cancellationToken);
    }
}
