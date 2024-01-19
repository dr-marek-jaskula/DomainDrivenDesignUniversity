using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shopway.Persistence.Cache;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.Registration;

public static class PersistenceLayerRegistration
{
    public static IServiceCollection RegisterPersistenceLayer(this IServiceCollection services, IHostEnvironment environment)
    {
        Console.WriteLine($"Seeding Persistence Layer Memory Cache: {PersistenceCache.SeedCache}");

        services
            .RegisterBackgroundServices()
            .RegisterDatabaseContext(environment.IsDevelopment())
            .RegisterCache()
            .RegisterRepositories()
            .RegisterDecorators()
            .RegisterHealthChecks()
            .RegisterMediator();

        return services;
    }

    public static IApplicationBuilder UsePersistenceLayer(this IApplicationBuilder app)
    {
        app
            .UseHealthChecks()
            .ApplyMigrations<ShopwayDbContext>();

        return app;
    }
}