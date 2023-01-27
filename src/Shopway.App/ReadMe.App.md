# App Layer

In this layer we:

- Register dependencies
- Set Middlewares
- Store Options

## Redis Cache

1. install NuGet Microsoft.Extensions.Caching.StackExchangeRedis
2. Create CacheOptions
3. Use caching, for instance in the decorator (see CachedProductRepository)
4. Add PrivateResolver (in Shopway.Persistance) to be able to deserialized object with private setters

IMemory cache is faster, but the Redis cache can be reused between multiple instance of the same application.

If we do not want to use decorator pattern for cache, we can create a CacheService or a CachePipeline

## Spin the Docker Container with Redis

Create and run the docker container from the redis images in the detached mode

```cmd
docker run -p 6379:6379 --name redis -d redis
```


