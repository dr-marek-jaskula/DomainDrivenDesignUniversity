using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Shopway.App.Options;
using Shopway.Persistence;
using Shopway.Persistence.Interceptors;

namespace Shopway.App.Registration;

public static class DatabaseContextRegistration
{
    public static void RegisterDatabaseContext(this IServiceCollection services, bool isDevelopment)
    {
        services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
        {
            var databaseOptions = serviceProvider.GetRequiredService<IOptions<DatabaseOptions>>().Value;

            var outboxInterceptor = serviceProvider.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;
            var auditableInterceptor = serviceProvider.GetService<UpdateAuditableEntitiesInterceptor>()!;

            optionsBuilder.UseSqlServer(databaseOptions.ConnectionString, options =>
            {
                options.CommandTimeout(databaseOptions.CommandTimeout);

                options.EnableRetryOnFailure(
                    databaseOptions.MaxRetryCount,
                    TimeSpan.FromSeconds(databaseOptions.MaxRetryDelay),
                    Array.Empty<int>());

            }).AddInterceptors(outboxInterceptor, auditableInterceptor);

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
    }
}