using Shopway.Infrastructure.Decoratos;
using MediatR;
using Shopway.Persistence.Repositories.Decorators;
using Shopway.Domain.Repositories;

namespace Shopway.App.Registration;

public static class DecoractorsRegistration
{
    public static void RegisterServiceDecorators(this IServiceCollection services)
    {
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandlerDecorator<>));
        services.Decorate<IOrderRepository, CachedOrderRepository>();
        services.Decorate<IProductRepository, CachedProductRepository>();
    }
}