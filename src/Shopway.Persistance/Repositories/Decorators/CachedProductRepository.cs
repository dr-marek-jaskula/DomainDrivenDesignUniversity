using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Shopway.Domain.Entities;
using Shopway.Domain.Extensions;
using Shopway.Domain.Repositories;
using System.Linq.Expressions;

namespace Shopway.Persistence.Repositories.Decorators;

public sealed class CachedProductRepository : IProductRepository
{
    private readonly IProductRepository _decorated;
    //basic way to implement cache
    //private readonly IMemoryCache _memoryCache;
    //Cache with Redis
    private readonly IDistributedCache _distributedCache;

    public CachedProductRepository(IProductRepository decorated, IDistributedCache distributedCache)
    {
        _decorated = decorated;
        _distributedCache = distributedCache;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        string key = $"product-{id}";

        //Basic memory cache
        //return _memoryCache.GetOrCreateAsync(
        //    key,
        //    entry =>
        //    {
        //        entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));

        //        return _decorated.GetByIdAsync(id, cancellationToken);
        //    });

        string? cachedProduct = await _distributedCache.GetStringAsync(
            key,
            cancellationToken);

        Product? product;

        if (cachedProduct.IsNullOrEmptyOrWhiteSpace())
        {
            product = await _decorated.GetByIdAsync(id, cancellationToken);

            if (product is null)
            {
                return product;
            }

            await _distributedCache.SetStringAsync(
                key, 
                JsonConvert.SerializeObject(product),
                cancellationToken);

            return product;
        }

        product = JsonConvert.DeserializeObject<Product>(
            cachedProduct,
            new JsonSerializerSettings
            {
                //Use private parameterless constructor to deserialize object
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });

        return product;
    }

    public Task<Product?> GetByIdWithIncludesAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<Product, object>>[] includes)
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
}

