using Shopway.Domain.Users;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Persistence.Outbox;
using Shopway.Persistence.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class RepositoriesRegistration
{
    internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

        return services;
    }
}
