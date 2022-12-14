using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Abstractions;
using Shopway.Persistence.Framework;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories;

public sealed class OrderRepository : BaseRepository, IOrderRepository
{
    public OrderRepository(ShopwayDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Order?> GetByIdAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        return await _dbContext
            .Set<Order>()
            .AsSplitQuery()
            .Include(x => x.Product)
            .Include(x => x.Payment)
            .Include(x => x.Customer)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<Order?> GetByIdWithIncludesAsync(OrderId id, CancellationToken cancellationToken = default, params Expression<Func<Order, object>>[] includes)
    {
        var baseQuery = _dbContext
            .Set<Order>()
            .AsQueryable();

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                baseQuery = baseQuery.Include(include);
            }
        }

        Order? order = await baseQuery
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return order;
    }

    public void Create(Order order)
    {
        _dbContext
            .Set<Order>()
            .Add(order);
    }

    public void Update(Order order)
    {
        _dbContext
            .Set<Order>()
            .Update(order);
    }

    public void Remove(Order order)
    {
        _dbContext
            .Set<Order>()
            .Remove(order);
    }
}
