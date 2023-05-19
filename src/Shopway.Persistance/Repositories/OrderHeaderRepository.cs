using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.Entities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class OrderHeaderRepository : RepositoryBase, IOrderHeaderRepository
{
    public OrderHeaderRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<OrderHeader> GetByIdAsync(OrderHeaderId id, CancellationToken cancellationToken)
    {
        return await _dbContext
            .Set<OrderHeader>()
            .Include(x => x.OrderLines)
                .ThenInclude(line => line.Product)
            .Include(x => x.Payment)
            .FirstAsync(x => x.Id == id, cancellationToken);
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

        if (includes.Any())
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
}
