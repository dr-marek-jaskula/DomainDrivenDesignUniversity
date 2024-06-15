using Shopway.Domain.Common.DataProcessing.Proxy;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Authorization;
using Shopway.Infrastructure.Outbox;
using Shopway.Persistence.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

internal static class RepositoriesRegistration
{
    internal static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrderHeaderRepository, OrderHeaderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
        services.AddScoped(typeof(IProxyRepository<,>), typeof(ProxyRepository<,>));
        services.AddScoped(typeof(IProxyWithKeyRepository<,,>), typeof(ProxyWithKeyRepository<,,>));

        return services;
    }
}
