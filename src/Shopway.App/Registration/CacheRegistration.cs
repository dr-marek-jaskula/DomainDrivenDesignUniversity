using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shopway.App.Options;

namespace Shopway.App.Registration;

//1. install NuGet Microsoft.Extensions.Caching.StackExchangeRedis
//2. Create CacheOptions (see Options)
//3. 
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
