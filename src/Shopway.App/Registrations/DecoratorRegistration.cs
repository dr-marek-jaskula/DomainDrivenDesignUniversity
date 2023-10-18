using MediatR;
using Shopway.Infrastructure.Decoratos;
using Shopway.Domain.Abstractions.Repositories;
using Shopway.Persistence.Repositories.Decorators;

namespace Microsoft.Extensions.DependencyInjection;

public static class DecoratorRegistration
{
    public static IServiceCollection RegisterDecorators(this IServiceCollection services)
    {
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandlerDecorator<>));

        services.Decorate<IOrderHeaderRepository, CachedOrderHeaderRepository>();
        services.Decorate<IProductRepository, CachedProductRepository>();

        return services;
    }
}