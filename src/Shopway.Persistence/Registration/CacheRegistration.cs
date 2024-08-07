﻿using Microsoft.Extensions.Caching.StackExchangeRedis;
using Shopway.Infrastructure.Options;
using ZiggyCreatures.Caching.Fusion;

namespace Microsoft.Extensions.DependencyInjection;

internal static class CacheRegistration
{
    private const string localhost = nameof(localhost);

    internal static IServiceCollection RegisterCache(this IServiceCollection services)
    {
        var cacheOptions = services.GetOptions<CacheOptions>();

        var fusionCacheBuilder = services.AddFusionCache()
            .WithOptions(options =>
            {
                options.DistributedCacheCircuitBreakerDuration = TimeSpan.FromSeconds(2);
            })
            .WithDefaultEntryOptions(options =>
            {
                options.Duration = TimeSpan.FromMinutes(1);
                options.FailSafeMaxDuration = TimeSpan.FromHours(2);
                options.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);

                options.FactorySoftTimeout = TimeSpan.FromMilliseconds(100);
                options.FactoryHardTimeout = TimeSpan.FromMilliseconds(1500);

                options.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1);
                options.DistributedCacheHardTimeout = TimeSpan.FromSeconds(2);
                options.JitterMaxDuration = TimeSpan.FromSeconds(2);
            });

        if (cacheOptions.ConnectionString!.Contains(localhost))
        {
            services.AddMemoryCache();
        }
        else
        {
            fusionCacheBuilder.WithDistributedCache(new RedisCache(new RedisCacheOptions()
            {
                Configuration = cacheOptions.ConnectionString!
            }));
        }

        //for adding Backplane we should install: "ZiggyCreatures.FusionCache.Backplane.StackExchangeRedis" and then add 
        //.WithBackplane(new RedisBackplane(new RedisBackplaneOptions() { Configuration = "CONNECTION STRING" });

        return services;
    }
}
