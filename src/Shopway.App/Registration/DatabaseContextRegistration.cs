using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shopway.Infrastructure.Options;
using Shopway.Persistence.Framework;

namespace Microsoft.Extensions.DependencyInjection;

public static class DatabaseContextRegistration
{
    public static IServiceCollection RegisterDatabaseContext(this IServiceCollection services, bool isDevelopment)
    {
        services.AddDbContextPool<ShopwayDbContext>((serviceProvider, optionsBuilder) =>
        {
            var databaseOptions = services.GetOptions<DatabaseOptions>();

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

        services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));

        return services;
    }
}