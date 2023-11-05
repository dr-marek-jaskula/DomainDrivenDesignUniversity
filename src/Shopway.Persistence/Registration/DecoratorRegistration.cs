using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Repositories.Decorators;

namespace Microsoft.Extensions.DependencyInjection;

public static class DecoratorRegistration
{
    internal static IServiceCollection RegisterDecorators(this IServiceCollection services)
    {
        services.Decorate<IOrderHeaderRepository, CachedOrderHeaderRepository>();
        services.Decorate<IProductRepository, CachedProductRepository>();

        return services;
    }
}