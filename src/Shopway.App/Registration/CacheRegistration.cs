using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shopway.App.Options;

namespace Shopway.App.Registration;

public static class CacheRegistration
{
    public static void RegisterCache(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var cacheOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

        services.AddStackExchangeRedisCache(redisOptions =>
        {
            string connection = cacheOptions.ConnectionString!;

            redisOptions.Configuration = connection;
        });
    }
}
