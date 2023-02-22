using Shopway.Infrastructure.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class CacheRegistration
{
    public static IServiceCollection RegisterCache(this IServiceCollection services)
    {
        var cacheOptions = services.GetOptions<CacheOptions>();

        services.AddStackExchangeRedisCache(redisOptions =>
        {
            string connection = cacheOptions.ConnectionString!;
            redisOptions.Configuration = connection;
        });

        return services;
    }
}
