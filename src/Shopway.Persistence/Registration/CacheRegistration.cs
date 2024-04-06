using Microsoft.Extensions.Caching.StackExchangeRedis;
using Shopway.Domain.Common.Utilities;
using Shopway.Infrastructure.Options;
using ZiggyCreatures.Caching.Fusion;
using ZiggyCreatures.Caching.Fusion.Serialization.NewtonsoftJson;

namespace Microsoft.Extensions.DependencyInjection;

public static class CacheRegistration
{
    private const string Redis = nameof(Redis);

    internal static IServiceCollection RegisterCache(this IServiceCollection services)
    {
        var cacheOptions = services.GetOptions<CacheOptions>();

        var fusionCacheBuilder = services.AddFusionCache()
            .WithDefaultEntryOptions(options =>
            {
                options.Duration = TimeSpan.FromMinutes(1);
                options.FailSafeMaxDuration = TimeSpan.FromHours(2);
                options.FailSafeThrottleDuration = TimeSpan.FromSeconds(30);

                options.FactorySoftTimeout = TimeSpan.FromMilliseconds(100);
                options.FactoryHardTimeout = TimeSpan.FromMilliseconds(1500);

                //options.DistributedCacheSoftTimeout = TimeSpan.FromSeconds(1);
                //options.DistributedCacheHardTimeout = TimeSpan.FromSeconds(2);
            })
            .WithSerializer(new FusionCacheNewtonsoftJsonSerializer());

        if (cacheOptions.ConnectionString!.Contains("localhost"))
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
