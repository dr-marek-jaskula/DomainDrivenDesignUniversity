﻿using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Common.DataProcessing;
using Shopway.Domain.Common.DataProcessing.Abstractions;
using Shopway.Domain.Orders;
using Shopway.Domain.Users;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications;
using Shopway.Persistence.Specifications.Common;
using Shopway.Persistence.Specifications.OrderHeaders;
using Shopway.Persistence.Utilities;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class OrderHeaderRepository(ShopwayDbContext dbContext) : IOrderHeaderRepository
{
    private readonly ShopwayDbContext _dbContext = dbContext;

    public async Task<bool> IsOrderHeaderCreatedByUser(OrderHeaderId id, UserId userId, CancellationToken cancellationToken)
    {
        var specification = OrderHeaderSpecification.ById.AndUserId.Create(id, userId);

        return await _dbContext
            .Set<OrderHeader>()
            .UseSpecification(specification)
            .AnyAsync(cancellationToken);
    }

    public async Task<OrderHeader> GetByIdAsync(OrderHeaderId id, CancellationToken cancellationToken)
    {
        var specification = OrderHeaderSpecification.ById.WithOrderLines.AndProducts.Create(id);

        return await _dbContext
            .Set<OrderHeader>()
            .UseSpecification(specification)
            .FirstAsync(cancellationToken);
    }

    public async Task<OrderHeader> GetByIdWithOrderLineAsync(OrderHeaderId id, OrderLineId orderLineId, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<OrderHeader>()
            .Include(x => x.OrderLines.Where(line => line.Id == orderLineId))
            .FirstAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<OrderHeader> GetByIdWithIncludesAsync(OrderHeaderId id, CancellationToken cancellationToken, params Expression<Func<OrderHeader, object>>[] includes)
    {
        var baseQuery = _dbContext
            .Set<OrderHeader>()
            .AsSplitQuery()
            .AsQueryable();

        if (includes.Length != 0)
        {
            foreach (var include in includes)
            {
                baseQuery = baseQuery.Include(include);
            }
        }

        return await baseQuery
            .FirstAsync(x => x.Id == id, cancellationToken);
    }

    public void Create(OrderHeader order)
    {
        _dbContext
            .Set<OrderHeader>()
            .Add(order);
    }

    public void Update(OrderHeader order)
    {
        _dbContext
            .Set<OrderHeader>()
            .Update(order);
    }

    public void Remove(OrderHeader order)
    {
        _dbContext
            .Set<OrderHeader>()
            .Remove(order);
    }

    public async Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>
    (
        IOffsetPage page,
        CancellationToken cancellationToken,
        IFilter<OrderHeader>? filter = null,
        IList<LikeEntry<OrderHeader>>? likes = null,
        ISortBy<OrderHeader>? sort = null,
        IMapping<OrderHeader, TResponse>? mapping = null,
        Expression<Func<OrderHeader, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<OrderHeader>>? buildIncludes = null
    )
    {
        var specification = CommonSpecification.WithMapping.Create<OrderHeader, OrderHeaderId, TResponse>
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
            .Set<OrderHeader>()
            .UseSpecification(specification)
            .PageAsync(page, cancellationToken);
    }

    public async Task<(IList<TResponse> Responses, Ulid Cursor)> PageAsync<TResponse>
    (
        ICursorPage page,
        CancellationToken cancellationToken,
        IFilter<OrderHeader>? filter = null,
        IList<LikeEntry<OrderHeader>>? likes = null,
        ISortBy<OrderHeader>? sort = null,
        IMapping<OrderHeader, TResponse>? mapping = null,
        Expression<Func<OrderHeader, TResponse>>? mappingExpression = null,
        Action<IIncludeBuilder<OrderHeader>>? buildIncludes = null
    )
        where TResponse : class, IHasCursor
    {
        Expression<Func<OrderHeader, bool>> cursorFilter = product => product.Id >= OrderHeaderId.Create(page.Cursor);

        var specification = CommonSpecification.WithMapping.Create<OrderHeader, OrderHeaderId, TResponse>
        (
            filter,
            cursorFilter,
            likes,
            sort,
            mapping,
            mappingExpression,
            buildIncludes: buildIncludes
        );

        return await _dbContext
            .Set<OrderHeader>()
            .UseSpecification(specification)
            .PageAsync(page, cancellationToken);
    }
}
