using Shopway.Persistence.Outbox;
using Shopway.Persistence.Repositories;
using Shopway.Domain.Abstractions.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class RepositoriesRegistration
{
    internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPermissionRepository, PermissionRepository>();

        return services;
    }
}
