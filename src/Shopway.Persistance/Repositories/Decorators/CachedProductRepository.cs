using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Shopway.Domain.Entities;
using Shopway.Domain.Utilities;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Resolvers;
using System.Linq.Expressions;
using Shopway.Persistence.Framework;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Domain.EntityBusinessKeys;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedProductRepository : IProductRepository
{
    //In order to reuse the same settings we store in a private static field
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new()
    {
        //Use private parameterless constructor to deserialize object
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
        //We also need to use PrivateResoler to be able to deal with properties private setters
        ContractResolver = new PrivateResolver()
    };

    private readonly IProductRepository _decorated;
    //Cache with Redis
    private readonly IDistributedCache _distributedCache;
    //This is if we need to track the entity, when it is obtained from the Redis Cache
    private readonly ShopwayDbContext _context;

    public CachedProductRepository(IProductRepository decorated, IDistributedCache distributedCache, ShopwayDbContext context)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
        _context = context;
    }

    public async Task<Product> GetByIdAsync(ProductId id, CancellationToken cancellationToken)
    {
        string key = $"product-{id}";

        string? cachedProduct = await _distributedCache.GetStringAsync(
            key,
            cancellationToken);

        Product product;

        if (cachedProduct.IsNullOrEmptyOrWhiteSpace())
        {
            product = await _decorated.GetByIdAsync(id, cancellationToken);

            await _distributedCache.SetStringAsync(
                key, 
                JsonConvert.SerializeObject(product),
                cancellationToken);

            return product;
        }

        product = JsonConvert.DeserializeObject<Product>(cachedProduct!, _jsonSerializerSettings)!;

        //Make EF Core track the obtained entity if it is not null
        _context.Set<Product>().Attach(product);

        return product;
    }

    public Task<Product> GetByIdWithIncludesAsync(ProductId id, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes)
    {
        return _decorated.GetByIdWithIncludesAsync(id, cancellationToken, includes);
    }

    public void Create(Product product)
    {
        _decorated.Create(product);
    }

    public void Remove(Product product)
    {
        _decorated.Remove(product);
    }

    public void Update(Product product)
    {
        _decorated.Update(product);
    }

    public Task<Product> GetByKeyAsync(ProductKey key, CancellationToken cancellationToken)
    {
        return _decorated.GetByKeyAsync(key, cancellationToken);
    }

    public Task<bool> AnyAsync(ProductKey key, CancellationToken cancellationToken)
    {
        return _decorated.AnyAsync(key, cancellationToken);
    }

    public Task<(IList<TResponse> Responses, int TotalCount)> PageAsync<TResponse>(IFilter<Product>? filter, ISortBy<Product>? sortBy, IPage page, Expression<Func<Product, TResponse>>? select, CancellationToken cancellationToken, params Expression<Func<Product, object>>[] includes)
    {
        return _decorated.PageAsync(filter, sortBy, page, select, cancellationToken, includes);
    }
}
