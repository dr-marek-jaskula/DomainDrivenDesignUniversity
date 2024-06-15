using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using Shopway.Persistence.Repositories.Decorators;

namespace Microsoft.Extensions.DependencyInjection;

internal static class DecoratorRegistration
{
    internal static IServiceCollection RegisterDecorators(this IServiceCollection services)
    {
        services.Decorate<IOrderHeaderRepository, CachedOrderHeaderRepository>();
        services.Decorate<IProductRepository, CachedProductRepository>();

        return services;
    }
}
