using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Shopway.App.Options;
using Shopway.Persistence.Framework;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseContextRegistration
{
    public static IServiceCollection RegisterDatabaseContext(this IServiceCollection services, bool isDevelopment)
    {
        services.AddDbContextPool<ShopwayDbContext>((serviceProvider, optionsBuilder) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            optionsBuilder.UseSqlServer(databaseOptions.ConnectionString!, options =>
            {
                options.CommandTimeout(databaseOptions.CommandTimeout);

                options.EnableRetryOnFailure(
                    databaseOptions.MaxRetryCount,
                    TimeSpan.FromSeconds(databaseOptions.MaxRetryDelay),
                    Array.Empty<int>());

            });

            if (isDevelopment)
            {
                optionsBuilder.EnableDetailedErrors();
                //optionsBuilder.EnableSensitiveDataLogging(); //DO NOT USE THIS IN PRODUCTIN! Used to get parameter values. DO NOT USE THIS IN PRODUCTIN!
                optionsBuilder.ConfigureWarnings(warningAction =>
                {
                    warningAction.Log(new EventId[]
                    {
                        CoreEventId.FirstWithoutOrderByAndFilterWarning,
                        CoreEventId.RowLimitingOperationWithoutOrderByWarning
                    });
                });
            }
        });

        return services;
    }
}