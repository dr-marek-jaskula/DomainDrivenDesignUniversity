using MediatR;
using Shopway.Infrastructure.Decoratos;

namespace Microsoft.Extensions.DependencyInjection;

public static class DecoratorRegistration
{
    internal static IServiceCollection RegisterDecorators(this IServiceCollection services)
    {
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandlerDecorator<>));

        return services;
    }
}