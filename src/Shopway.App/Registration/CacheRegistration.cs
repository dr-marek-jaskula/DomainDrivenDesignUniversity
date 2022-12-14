using Microsoft.Extensions.Options;
using Shopway.App.Options;

namespace Microsoft.Extensions.DependencyInjection;

public static class CacheRegistration
{
    public static IServiceCollection RegisterCache(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var cacheOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

        services.AddStackExchangeRedisCache(redisOptions =>
        {
            string connection = cacheOptions.ConnectionString!;

            redisOptions.Configuration = connection;
        });

        return services;
    }
}
