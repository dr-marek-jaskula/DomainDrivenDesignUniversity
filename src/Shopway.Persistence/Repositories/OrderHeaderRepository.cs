using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Orders;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Specifications;
using Shopway.Persistence.Specifications.OrderHeaders;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class OrderHeaderRepository(ShopwayDbContext dbContext) : IOrderHeaderRepository
{
    private readonly ShopwayDbContext _dbContext = dbContext;

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
}
