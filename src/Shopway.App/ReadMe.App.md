# App Layer (User Interface (Web/Api) Layer)

In this project we store components connected to :

- Registering services
- Middlewares
- Options
- Settings

## Quartz background jobs

To configure Quartz background jobs we need Quartz.Extensions.Hosting NuGet Package. This integrates with background service in ASP.NET.

## Redis Cache

1. install NuGet Microsoft.Extensions.Caching.StackExchangeRedis
2. Create CacheOptions (see Options)
3. Use caching in the decorator (see CachedProductRepository)
4. Add PrivateResolver (in Shopway.Persistance) to be able to deserialized object with private setters

IMemory cache is faster, but the Redis cache can be resued between multiple instance of our application.

If we do not want to use decorator pattern for cache, we can use create a CacheService

## Spin the Docker Container with Redis

Create and run the docker container from the redis images in the detached mode

```cmd
docker run -p 6379:6379 --name redis -d redis
```


